using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ODataErrors;

namespace TecCore.Services.Entra
{
    public class DeviceService : MsGraphService
    {
        public DeviceService() : base() { }
        
        protected async Task<IEnumerable<Device>> FetchDevicesAsync(string? filter = null)
        {
            var devices = new List<Device>();
            string _filter;
            try
            {
                _filter = string.IsNullOrEmpty(filter)
                    ? "startswith(displayName, 'CTE')"
                    : $"startswith(displayName, 'CTE') and ({filter})";

                var results = await _graphClient.Devices.GetAsync(request =>
                {
                    request.QueryParameters.Filter = _filter;
                    request.QueryParameters.Orderby = new[] { "displayName" };
                    request.QueryParameters.Count = true;
                    request.QueryParameters.Select = DeviceProperties;
                    request.Headers.Add("ConsistencyLevel", "eventual");
                });

                if (results?.Value != null)
                    devices.AddRange(results.Value);

                if (results != null)
                {
                    var pageIterator = PageIterator<Device, DeviceCollectionResponse>.CreatePageIterator(
                        _graphClient, results, device =>
                        {
                            devices.Add(device);
                            return true;
                        });

                    await pageIterator.IterateAsync();
                }
            }
            catch (ODataError ex)
            {
                Console.WriteLine($"Error fetching devices: {ex.Error?.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error fetching devices: {ex.Message}");
            }

            return devices;
        }

        /// <summary>
        /// Retrieves all devices where displayName starts with "CTE".
        /// </summary>
        public async Task<IEnumerable<Device>> GetAllDevicesAsync() =>
            await FetchDevicesAsync();

        /// <summary>
        /// Retrieves devices filtered by asset tag.
        /// </summary>
        public async Task<IEnumerable<Device>> GetDevicesByAssetTagAsync(string assetTag) =>
            await FetchDevicesAsync($"endswith(displayName, '{assetTag}')");

        /// <summary>
        /// Retrieves devices filtered by classroom.
        /// </summary>
        public async Task<IEnumerable<Device>> GetDevicesByClassroomAsync(string classroom)
        {
            var devices = await FetchDevicesAsync();
            var enumerable = devices as Device[] ?? devices.ToArray();
            return (enumerable.Length == 0)
                ? new List<Device>()
                : enumerable.Where(i => i.DisplayName!.Substring(4, 3) == classroom);
        }

        /// <summary>
        /// Retrieves a specific device by device name.
        /// </summary>
        public async Task<Device> GetDeviceByDeviceNameAsync(string deviceName)
        {
            var devices = await FetchDevicesAsync($"displayName eq '{deviceName}'");
            return devices.ToArray()[0];
        }

        /// <summary>
        /// Updates device information.
        /// </summary>
        public async Task UpdateDevice(Device device)
        {
            await _graphClient.Devices[device.Id].PatchAsync(device);
        }

        /// <summary>
        /// Retrieves a specific device by ID.
        /// </summary>
        public async Task<Device?> GetDeviceAsync(string deviceId) =>
            await _graphClient.Devices[deviceId].GetAsync();
    }
}