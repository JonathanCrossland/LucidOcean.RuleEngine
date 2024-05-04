/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/

namespace LucidOcean.RuleEngine.Context
{
    public interface IConfigurationAccessor
    {
        string GetValue(string settingName);

        bool SetValue(string settingName, string value);

    }
}
