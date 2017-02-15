﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ThemePack.Common.Abstractions;
using ThemePack.Common.BusinessModels;
using ThemePack.Common.Extentions;
using ThemePack.Common.Helpers;
using ThemePack.Models.Models;
using ThemePack.Models.Models.Enums;

namespace ThemePack.Common.ThemeManagement
{
    public class DllThemeSeeker : IThemesSeeker
    {
        public IList<ThemeM> GetThemes(string folder)
        {
            var themes = new List<ThemeM>();

            if (Directory.Exists(folder))
            {
                var  dllFileNames = Directory.GetFiles(folder, "*.dll");
                //ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Length);
                foreach (string dllFile in dllFileNames)
                {
                    var assemblyName = AssemblyName.GetAssemblyName(dllFile).Name;
                    var currentDllResources = AssemblyResourceHelper.LoadXaml(dllFile);
                    themes.Add(new ThemeM(assemblyName, currentDllResources.Where(_ => _.ResourceType == XamlResourceType.ColorScheme).Select(it => new ColorSchemeM(it.Name,it.Resource, it.Uri)).ToList(),
                                          currentDllResources.Where(_ => _.ResourceType == XamlResourceType.ControlStyle).Select(it => new ControlStyleM(it.Name, it.Resource, it.Uri)).ToList()));
                }
            }

            //List<string> resourceNames = new List<string>();
            //var assembly = Assembly.GetExecutingAssembly();
            //var rm = new ResourceManager(assembly.GetName().Name + ".g", assembly);
            //try
            //{
            //    var list = rm.GetResourceSet(CultureInfo.CurrentCulture, true, true);
            //    foreach (DictionaryEntry item in list)
            //        resourceNames.Add((string)item.Key);
            //}
            //finally
            //{
            //    rm.ReleaseAllResources();
            //}
            return themes;
        }


    }
}