using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using BulletEngineNeo.Bullet;
using MoonSharp.Interpreter;
using BulletEngineNeo.FileSystem;


namespace BulletEngineNeo.ScriptEffects
{
    public class EffectFunctionStorage
    {
        Dictionary<string ,string> mLuaFiles;
        Script script;
        bool mHasChanged;
        string mChangedFile;

        public EffectFunctionStorage()
        {
            mChangedFile = "";
            mHasChanged = false;
        }

        public void Initialize(string path)
        {
            //UserData.RegistrationPolicy = InteropRegistrationPolicy.Automatic;
            //UserData.DefaultAccessMode = InteropAccessMode.;

            mLuaFiles = new Dictionary<string, string>();
            string[] lNames = System.IO.Directory.GetFiles(path);
            for (int i = 0; i < lNames.Length; i++)
            {
                mLuaFiles.Add(Path.GetFileNameWithoutExtension(lNames[i]), File.ReadAllText(lNames[i]));
            }
            AddFileWatcher(path);
            InitializeScripts();
        }

        public void Update()
        {
            if (mHasChanged)
            {
                refreshFunction(mChangedFile);
                mHasChanged = false;
            }
        }

        private void refreshFunction(string path)
        {
            bool open = false;
            do
            {
                open = IsFileReady(path);
            } while (!open);
            string newStr = File.ReadAllText(path);
            script.DoString(newStr);
        }

        private void InitializeScripts()
        {
            script = new Script();
            foreach (var luaString in mLuaFiles)
            {
                script.DoString(luaString.Value);
            }
        }

        public EffectFunction GetEffectFunction(string name)
        {
            return (DynValue obj, MainGameLogic game) => 
            {
                script.Call(script.Globals[name+"_effect"], obj);
            };
        }

        private void AddFileWatcher(string path)
        {
            DirectoryMonitor monitor = new DirectoryMonitor(path);
            monitor.Change += OnChange;
            monitor.Start();
        }

        private void OnChange(string path)
        {
            mHasChanged = true;
            mChangedFile = path;
        }

        private static bool IsFileReady(string filename)
        {
            try
            {
                using (FileStream input = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    return input.Length > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
