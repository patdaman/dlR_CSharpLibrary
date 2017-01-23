using AppDataLib.Exceptions;
using CommonUtils.AppConfiguration;
using DataLayer;
using ViewModel;
using ViewModel.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using EF = EFDataModel;

namespace BusinessLayer.Events
{
    public class EventsProcessor : BehaviorBase
    {
        public EventsProcessor():base()
        {
            var config = new AppConfiguration();
            config.AddProvider(new ConfigFileConfigProvider());
            config.AddProvider(new EnvironmentVariableConfigProvider(EnvironmentVariableTarget.User));
            IRepository<EF.SGNL_ANALYTICS.SignalEvent> ANALYTICSrepo;
        }

        public EventsProcessor( string analyticsConnectionString) : base()
        {

            AnalyticsDbContext = new EFDataModel.SGNL_ANALYTICS.SGNL_ANALYTICSEntities(analyticsConnectionString);
        }

        public EventsProcessor(EFDataModel.SGNL_ANALYTICS.SGNL_ANALYTICSEntities dbContext)
        {
            EF.SGNL_ANALYTICS.SGNL_ANALYTICSEntities AnalyticsDbContext = new EFDataModel.SGNL_ANALYTICS.SGNL_ANALYTICSEntities();
            AnalyticsDbContext = dbContext;
        }
        
        public static List<ViewModel.Events.Event> FilterEvents(List<ViewModel.Events.Event> fullList)
        {
            // Add Filter Here:
            return fullList;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Converts the events to an ef. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20161003. </remarks>
        ///
        /// <param name="events">   The events. </param>
        ///
        /// <returns>   The given data converted to an ef. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static List<EF.SGNL_ANALYTICS.SignalEvent> ConvertToEf(List<ViewModel.Events.Event> events)
        {
            EF.SGNL_ANALYTICS.SGNL_ANALYTICSEntities Analytics = new EF.SGNL_ANALYTICS.SGNL_ANALYTICSEntities();
            SignalEFDataModel.SGNL_ANALYTICS.SignalEvent signalEvent = new EF.SGNL_ANALYTICS.SignalEvent();
            List<SignalEFDataModel.SGNL_ANALYTICS.SignalEvent> eventList = new List<SignalEFDataModel.SGNL_ANALYTICS.SignalEvent>();
            SignalEFDataModel.SGNL_ANALYTICS.EventType eventType = new SignalEFDataModel.SGNL_ANALYTICS.EventType();
            List<SignalEFDataModel.SGNL_ANALYTICS.EventType> eventTypeList = new List<SignalEFDataModel.SGNL_ANALYTICS.EventType>();
            
            eventTypeList = (from e in Analytics.EventTypes
                           where e.IsActive == true
                           select e).ToList();
            foreach (ViewModel.Events.Event changeEvent in events)
            {

                eventType = (from x in eventTypeList
                                 where x.EventDefinition == changeEvent.EventDefinition
                                 select x).FirstOrDefault();
                signalEvent.EventName = changeEvent.EventName;
                signalEvent.EventContent = changeEvent.EventContent;
                signalEvent.MessageSentDate = changeEvent.MessageSentDate;
                signalEvent.Source = changeEvent.Source;

                signalEvent.EventType = eventType;
                signalEvent.TypeId = eventType.id;
                
                signalEvent.ContentType = "string";
                signalEvent.EventContent = changeEvent.ToString();
                eventList.Add(signalEvent);
            }
            return eventList;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Saves the events. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20161003. </remarks>
        ///
        /// <exception cref="CustomException">  Thrown when a Signal error condition occurs. </exception>
        ///
        /// <param name="events">   The events. </param>
        ///-------------------------------------------------------------------------------------------------

        public static void SaveEvents(IEnumerable<ViewModel.Events.Event> events)
        {
            EF.SGNL_ANALYTICS.SGNL_ANALYTICSEntities AnalyticsDbContext = new SignalEFDataModel.SGNL_ANALYTICS.SGNL_ANALYTICSEntities();
            EF.SGNL_ANALYTICS.SignalEvent signalEvent = new EF.SGNL_ANALYTICS.SignalEvent();
            EF.SGNL_ANALYTICS.EventType eventType = new EF.SGNL_ANALYTICS.EventType();

            var eventFields = (from e in AnalyticsDbContext.EventTypes
                           where e.IsActive == true
                           select e).ToList();

            foreach (var newEvent in events)
            {
                eventType = (from x in eventFields
                             where x.EventDefinition == newEvent.EventDefinition
                             select x).FirstOrDefault();
                if (eventType != null)
                {
                    AnalyticsDbContext.SignalEvents.Add(new EFDataModel.SGNL_ANALYTICS.SignalEvent()
                        {
                            TypeId = eventType.id,
                            EventType = eventType,
                            EventName = newEvent.EventName,
                            EventContent = newEvent.EventContent,
                            ContentType = newEvent.ContentType,
                            Source = newEvent.Source ?? "",
                            MessageSentDate = newEvent.MessageSentDate ?? DateTime.UtcNow
                        }
                    );
                }
            }
            try
            {
                AnalyticsDbContext.SaveChanges();
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CustomException(CustomExceptionTypes.CreateFailure_Error,
                    "Error in creating Events ", ex);
            }
        }

         ///-------------------------------------------------------------------------------------------------
         /// <summary>  Gets signal event. </summary>
         ///
         /// <remarks>  Pdelosreyes, 20161003. </remarks>
         ///
         /// <exception cref="SgnlEventExceptions"> Thrown when a signal event exceptions error condition
         ///                                        occurs. </exception>
         ///
         /// <param name="eventDefinition"> The event definition. </param>
         ///
         /// <returns>  The signal event. </returns>
         ///-------------------------------------------------------------------------------------------------

         public static EF.SGNL_ANALYTICS.EventType GetSGNLEvent(string eventDefinition)
         {
             EF.SGNL_ANALYTICS.SGNL_ANALYTICSEntities AnalyticsDbContext = new SignalEFDataModel.SGNL_ANALYTICS.SGNL_ANALYTICSEntities();
             EF.SGNL_ANALYTICS.EventType et = new EF.SGNL_ANALYTICS.EventType();
             et = AnalyticsDbContext.EventTypes.Where(e => e.EventDefinition == eventDefinition).FirstOrDefault();
             if (et == null)
                 throw new SgnlEventExceptions("Event Type not found for " + eventDefinition);
 
             return et;
         }

         ///-------------------------------------------------------------------------------------------------
         /// <summary>  Populate clin signal event. </summary>
         ///
         /// <remarks>  Pdelosreyes, 20161003. </remarks>
         ///
         /// <param name="eventDefinition"> The event definition. </param>
         /// <param name="caseNumber">      The case number. </param>
         /// <param name="content">         The content. </param>
         /// <param name="source">          Source for the. </param>
         ///
         /// <returns>  A SignalViewModel.Events.SgnlEvent. </returns>
         ///-------------------------------------------------------------------------------------------------

         public static ViewModel.Events.Event PopulateClinSgnlEvent(string eventDefinition, string caseNumber, string content, string source)
         {
             SignalEFDataModel.SGNL_ANALYTICS.EventType eventType = new SignalEFDataModel.SGNL_ANALYTICS.EventType();
 
             eventType = GetSGNLEvent(eventDefinition);
             ViewModel.Events.Event sgEvent = new ViewModel.Events.Event()
             {
                 TypeId = eventType.id,
                 EventName = eventType.EventName,
                 MessageSentDate = DateTime.Now,
                 EventDefinition = eventType.EventDefinition,
                 EventContent = content,
                 Priority = eventType.Priority,
                 Source = source,
                 ContentType = "json",
                 IdField = caseNumber
             };
 
             return sgEvent;
     }
    }
}
