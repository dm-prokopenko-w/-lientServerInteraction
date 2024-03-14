using Game.Configs;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Core
{
    public class AssetLoader
    {
        private List<Config> _configs = new List<Config>();

        public async Task<Config> LoadConfig(string configId)
        {
            var currentConfig = _configs.Find(x => x.Id.Equals(configId));

            if (currentConfig != null) return currentConfig;

            var handle = Addressables.LoadAssetAsync<Config>(Constants.ConfigsPath + configId);
            await handle.Task;
            _configs.Add(handle.Result);
            return handle.Result;
        }

        public async Task<T> LoadAsset<T>(string path)
        {
            var handle = Addressables.LoadAssetAsync<T>(path);
            await handle.Task;
            return handle.Result;
        }
    }
}