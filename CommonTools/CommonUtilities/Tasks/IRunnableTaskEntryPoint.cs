///-------------------------------------------------------------------------------------------------
// <copyright file="RunnableTaskEntryPoint.cs" company="Signal Genetics Inc.">
// Copyright (c) 2016 Signal Genetics Inc.. All rights reserved.
// </copyright>
// <date>20160114</date>
// <summary>Implements the runnable task entry point class. This class is extended by any class 
// that wants to be executed by a dynamic load from a dll</summary>
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.Tasks
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    /// Values that represent runnable task status. The task runner may perform different operations
    /// depending on what the return status is from each of the task operations.
    /// </summary>
    ///
    ///-------------------------------------------------------------------------------------------------

    public enum RunnableTaskStatus
    {
        /// <summary>   An enum constant representing the task ready to start option. </summary>
        TaskReadyToStart,

        /// <summary>   An enum constant representing the task start complete option. </summary>
        TaskStartComplete,

        /// <summary>   An enum constant representing the task start failed option. </summary>
        TaskStartFailed,

        /// <summary>   An enum constant representing the task run complete option. </summary>
        TaskRunComplete,

        /// <summary>   An enum constant representing the task run failed option. </summary>
        TaskRunFailed,

        /// <summary>   An enum constant representing the task request rerun option. </summary>
        TaskRequestRerun,

        /// <summary>   An enum constant representing the task request reset option. </summary>
        TaskRequestReset,

        /// <summary>   An enum constant representing the task stop complete option. </summary>
        TaskStopComplete,

        /// <summary>   An enum constant representing the task stop failed option. </summary>
        TaskStopFailed,

        /// <summary>   An enum constant representing the task parameters read complete option. </summary>
        TaskParametersReadComplete,

        /// <summary>   An enum constant representing the task parameters read failed option. </summary>
        TaskParametersReadFailed,

        /// <summary>   An enum constant representing the task not initialized option. </summary>
        TaskNotInitialized,

        /// <summary>   An enum constant representing the task set configuration complete option. </summary>
        TaskSetConfigComplete,

        /// <summary>   An enum constant representing the task set configuration failed option. </summary>
        TaskSetConfigFailed
    };

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A runnable task entry point. </summary>
    ///
    /// <remarks>   Ssur, 20160114. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface IRunnableTaskEntryPoint
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// This is called by the task runner to execute the main body of the task. It is called after
        /// OnStartTask returns.
        /// </summary>
        ///
        /// <returns>   The RunnableTaskStatus. </returns>
        ///-------------------------------------------------------------------------------------------------

        RunnableTaskStatus RunTask();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Called by the task runner to set up the task using the default constructor. Note that this is
        /// called before RunTask.
        /// </summary>
        ///
        /// <returns>   The RunnableTaskStatus. </returns>
        ///-------------------------------------------------------------------------------------------------

        RunnableTaskStatus OnStartTask();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Called by the task runner after the RunTask returns. </summary>
        ///
        /// <returns>   The RunnableTaskStatus. </returns>
        ///-------------------------------------------------------------------------------------------------

        RunnableTaskStatus OnStopTask();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sets the parameters. The task runner passes along parameters as a string. The called code is
        /// responsible for handling of the parameter string.
        /// </summary>
        ///
        /// <param name="paramstr"> The paramstr. </param>
        ///
        /// <returns>   The RunnableTaskStatus. </returns>
        ///-------------------------------------------------------------------------------------------------

        RunnableTaskStatus SetParams(String paramstr);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sets task configuration using a.net configuration file. This should be able to handle the
        /// loaded app config as well.
        /// </summary>
        ///
        /// <param name="CfgFilePath">  Full pathname of the configuration file or null if the loaded app
        ///                             config should be used. If no aregument is passed in the default
        ///                             is null. </param>
        ///
        /// <returns>   The RunnableTaskStatus. </returns>
        ///-------------------------------------------------------------------------------------------------

        RunnableTaskStatus SetTaskConfig(String CfgFilePath=null);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets task name. </summary>
        ///
        /// <returns>   The task name. </returns>
        ///-------------------------------------------------------------------------------------------------
        String GetTaskName();

    }
}
