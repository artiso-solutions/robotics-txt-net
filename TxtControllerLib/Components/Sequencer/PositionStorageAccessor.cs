using System;
using System.Collections.Generic;
using System.IO;
using log4net;
using Newtonsoft.Json;
using RoboticsTxt.Lib.Contracts;
using RoboticsTxt.Lib.Contracts.Configuration;

namespace RoboticsTxt.Lib.Components.Sequencer
{
    internal class PositionStorageAccessor
    {
        private readonly ApplicationConfiguration applicationConfiguration;
        private readonly ILog logger = LogManager.GetLogger(typeof(PositionStorageAccessor));

        public PositionStorageAccessor(ApplicationConfiguration applicationConfiguration)
        {
            this.applicationConfiguration = applicationConfiguration;
        }

        private const string FileNamePattern = "Positions_{0}.json";

        public bool WritePositionsToFile(IEnumerable<Position> positions)
        {
            try
            {
                var positionsJson = JsonConvert.SerializeObject(positions, Formatting.Indented);

                var stream = new FileStream(string.Format(FileNamePattern, applicationConfiguration.ApplicationName), FileMode.Create);
                var streamWriter = new StreamWriter(stream);

                streamWriter.Write(positionsJson);
                streamWriter.Flush();
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
                return false;
            }

            return true;
        }

        public List<Position> LoadPositionsFromFile()
        {
            var positions = new List<Position>();

            try
            {
                var stream = new FileStream(string.Format(FileNamePattern, applicationConfiguration.ApplicationName), FileMode.Open);
                var streamReader = new StreamReader(stream);

                var positionsJson = streamReader.ReadToEnd();

                positions = JsonConvert.DeserializeObject<List<Position>>(positionsJson);
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
            }

            return positions;
        }
    }
}
