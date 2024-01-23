using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;

/// <summary>
/// Configuration class for data-related settings.
/// </summary>
internal static class Config
{
    /// <summary>
    /// The XML identifier for the data configuration.
    /// </summary>
    /// 
    static string s_data_config_xml = "data-config";

    /// <summary>
    /// Gets the next available task ID and increases the counter.
    /// </summary>
    /// 
    internal static int NextTaskId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextTaskId"); }

    /// <summary>
    /// Gets the next available dependence ID and increases the counter.
    /// </summary>
    internal static int NextDependenceId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextDependenceId"); }


}
