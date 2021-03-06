﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;


namespace RevitReactionImporter
{
    
    public class RAMModel
    {
        public List<RAMBeam> RamBeams {get; set;}
        public double[] OriginRAM { get; set; }
        public int StoryCount { get; set; }
        public List<Story> Stories { get; set; }
        public List<RAMGrid> Grids { get; set; }
        public List<RAMGrid> LetteredGrids { get; set; }
        public List<RAMGrid> NumberedGrids { get; set; }
        public List<RAMGrid> HorizontalGrids { get; set; }
        public List<RAMGrid> VerticalGrids { get; set; }
        public List<RAMGrid> OtherGrids { get; set; }
        public double[] ReferencePointDataTransfer { get; set; }
        public DesignCode UserSetDesignCode { get; set; }


        public RAMModel(DesignCode designCode)
        {
            RamBeams = new List<RAMBeam>();
            OriginRAM = new double[3];
            Stories = new List<Story>();
            Grids = new List<RAMGrid>();
            HorizontalGrids = new List<RAMGrid>();
            VerticalGrids = new List<RAMGrid>();
            LetteredGrids = new List<RAMGrid>();
            NumberedGrids = new List<RAMGrid>();
            OtherGrids = new List<RAMGrid>();
            UserSetDesignCode = designCode;
        }

        public enum GridDirectionalityClassification
        {
            None,
            Increasing,
            Decreasing
        }

        public class Story
        {
            public int Level { get; set; }
            public string StoryLabel { get; set; }
            public string LayoutType { get; set; }
            public double Height { get; set; }
            public double Elevation { get; set; }
            public bool MapRevitLevelToThis { get; set; }

            public Story(int level, string storyLabel, string layoutType, double height, double elevation)
            {
                Level = level;
                StoryLabel = storyLabel;
                LayoutType = layoutType;
                Height = height;
                Elevation = elevation;
                MapRevitLevelToThis = true;
            }
        }

        public class RAMGrid
        {
            public string Name { get; set; }
            public double Location { get; set; }
            public GridTypeNamingClassification GridTypeNaming { get; set; }
            public GridDirectionalityClassification DirectionalityClassification {get; set;}
            public GridOrientationClassification GridOrientation { get; set; }

            public RAMGrid (string name, double location)
            {
                Name = name;
                Location = location;
                GridTypeNaming = GridTypeNamingClassification.None;
                DirectionalityClassification = GridDirectionalityClassification.None;
                GridOrientation = GridOrientationClassification.None;


            }

            public enum GridOrientationClassification
            {
                None,
                Vertical,
                Horizontal,
                Other
            }

            public enum GridTypeNamingClassification
            {
                Lettered,
                Numbered,
                None
            }

        }

        public class RAMBeam
        {
            public string FloorLayoutType { get; set; }
            public string Size { get; set; }
            public bool IsCantilevered { get; set; }
            public double [] StartPoint { get; set; }
            public double[] EndPoint { get; set; }
            public double StartTotalReactionPositive { get; set; }
            public double EndTotalReactionPositive { get; set; }
            public double StartDeadLoadReactionPositive { get; set; }
            public double EndDeadLoadReactionPositive { get; set; }
            public double StartLiveLoadReactionPositive { get; set; }
            public double EndLiveLoadReactionPositive { get; set; }
            public int Id { get; set; }
            public bool IsMappedToRevitBeam { get; set; }
            public int StudCount { get; set; }
            public double Camber { get; set; }


            public RAMBeam(string floorLayoutType, string size, double startPointX, double startPointY, double endPointX, double endPointY, double startDeadLoadReactionPositive, double endDeadLoadReactionPositive, double startLiveLoadReactionPositive, double endLiveLoadReactionPositive)
            {
                StartPoint = new double[3];
                EndPoint = new double[3];
                FloorLayoutType = floorLayoutType;
                Size = size;
                StartPoint[0] = startPointX;
                StartPoint[1] = startPointY;
                EndPoint[0] = endPointX;
                EndPoint[1] = endPointY;
                StartDeadLoadReactionPositive = startDeadLoadReactionPositive;
                EndDeadLoadReactionPositive = endDeadLoadReactionPositive;
                StartLiveLoadReactionPositive = startLiveLoadReactionPositive;
                EndLiveLoadReactionPositive = endLiveLoadReactionPositive;
                IsCantilevered = false;
                IsMappedToRevitBeam = false;
            }
        }

        // Defines RAM reference point for model geometry mapping from RAM to Revit. Hard-coded as Grid A-1.
        public static double[] EstablishReferencePoint(List<RAMGrid> grids, List<RAMGrid> letteredGrids, List<RAMGrid> numberedGrids)
        {
            var referencePoint = new double[3];
            int letteredGridIndex = -1;
            int numberedGridIndex = -1;

            var letteredGrid = letteredGrids[0];
            var numberedGrid = numberedGrids[0];

            if (letteredGrid.GridOrientation == RAMGrid.GridOrientationClassification.Horizontal)
            {
                letteredGridIndex = 1;
            }
            else if (letteredGrid.GridOrientation == RAMGrid.GridOrientationClassification.Vertical)
            {
                letteredGridIndex = 0;
            }
            else
            {
                throw new Exception("TODO: Other Lettered Grid Classification");
            }

            if (numberedGrid.GridOrientation == RAMGrid.GridOrientationClassification.Horizontal)
            {
                numberedGridIndex = 1;
            }
            else if (numberedGrid.GridOrientation == RAMGrid.GridOrientationClassification.Vertical)
            {
                numberedGridIndex = 0;
            }
            else
            {
                throw new Exception("TODO: Other Numbered Grid Classification");
            }
            if(numberedGridIndex == letteredGridIndex)
            {
                throw new Exception("Numbered & Lettered Grids are parallel");
            }

            referencePoint[letteredGridIndex] = letteredGrid.Location;
            referencePoint[numberedGridIndex] = numberedGrid.Location;
            referencePoint[2] = 0.0;
            return referencePoint;
        }

        public static void PopulateAdditionalRAMModelInfo(RAMModel ramModel)
        {
            ClassifyGridDirectionalities(ramModel.Grids, ramModel.NumberedGrids, ramModel.LetteredGrids);
            ramModel.ReferencePointDataTransfer = EstablishReferencePoint(ramModel.Grids, ramModel.LetteredGrids, ramModel.NumberedGrids);
        }

        public static void ExecutePythonScript(List<string> filePaths)
        {
            for(int i=0; i < filePaths.Count; i++)
            {
                filePaths[i] = "\"" + filePaths[i] + "\"";
            }
            const string argsSeparator = " ";
            string args = string.Join(argsSeparator, filePaths);

            Process process = new Process();

            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WorkingDirectory = @"C:\dev\RAM Reaction Importer\RAM-Reaction-Importer",
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized,
                FileName = "cmd.exe",
                Arguments = string.Format("{0} {1}", "getLevels.py", args),
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true
        };
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            process.StartInfo = startInfo;
            process.Start();
            process.StandardInput.WriteLine(@"C:\dev\RAM Reaction Importer\RAM-Reaction-Importer");
            process.StandardInput.WriteLine("python getLevels.py " + args);
            // Read the standard output of the app we called.  
            // in order to avoid deadlock we will read output first 
            // and then wait for process terminate: 
            StreamReader myStreamReader = process.StandardOutput;
            string myString = myStreamReader.ReadLine();

            process.Close();

            // Write the output we got from python app.
            Console.WriteLine("Value received from script: " + myString);
            System.Threading.Thread.Sleep(2000);

        }

        private static List<double> ChooseLoadFactorsBasedOnDesignCode(DesignCode designCode)
        {
            var factors = new List<double>();
            if (designCode == DesignCode.ASD)
            {
                double deadLoadFactorASD1 = 1.0;
                double liveLoadFactorASD1 = 1.0;
                factors.Add(deadLoadFactorASD1);
                factors.Add(liveLoadFactorASD1);
            }
            else if (designCode == DesignCode.LRFD)
            {
                double deadLoadFactorLRFD1 = 1.4;
                double deadLoadFactorLRFD2 = 1.2;
                double liveLoadFactorLRFD1 = 0.0;
                double liveLoadFactorLRFD2 = 1.6;
                factors.Add(deadLoadFactorLRFD1);
                factors.Add(liveLoadFactorLRFD1);
                factors.Add(deadLoadFactorLRFD2);
                factors.Add(liveLoadFactorLRFD2);
            }
            else throw new Exception("Need to debug: Design Code not specified.");

            return factors;
        }

        private static void ApplyLoadFactors(DesignCode designCode, List<double> loadFactors, RAMBeam ramBeam)
        {
            if(designCode == DesignCode.ASD)
            {
                ramBeam.StartTotalReactionPositive = (loadFactors[0] * ramBeam.StartDeadLoadReactionPositive) + (loadFactors[1] * ramBeam.StartLiveLoadReactionPositive);
                ramBeam.EndTotalReactionPositive = (loadFactors[0] * ramBeam.EndDeadLoadReactionPositive) + (loadFactors[1] * ramBeam.EndLiveLoadReactionPositive);

            }
            if (designCode == DesignCode.LRFD)
            {
                double startTotalReactionPositive1 = (loadFactors[0] * ramBeam.StartDeadLoadReactionPositive) + (loadFactors[1] * ramBeam.StartLiveLoadReactionPositive);
                double endTotalReactionPositive1 = (loadFactors[0] * ramBeam.EndDeadLoadReactionPositive) + (loadFactors[1] * ramBeam.EndLiveLoadReactionPositive);
                double startTotalReactionPositive2 = (loadFactors[2] * ramBeam.StartDeadLoadReactionPositive) + (loadFactors[3] * ramBeam.StartLiveLoadReactionPositive);
                double endTotalReactionPositive2 = (loadFactors[2] * ramBeam.EndDeadLoadReactionPositive) + (loadFactors[3] * ramBeam.EndLiveLoadReactionPositive);

                ramBeam.StartTotalReactionPositive = Math.Max(Math.Ceiling(startTotalReactionPositive1), Math.Ceiling(startTotalReactionPositive2));
                ramBeam.EndTotalReactionPositive = Math.Max(Math.Ceiling(endTotalReactionPositive1), Math.Ceiling(endTotalReactionPositive2));

            }
        }

        public static RAMModel DeserializeRAMModel(DesignCode designCode)
        {
            List<double> loadFactors = ChooseLoadFactorsBasedOnDesignCode(designCode);
            var ramModel = new RAMModel(designCode);

            // TODO: Beam Data in its own function.
            string path = @"C:\dev\RAM Reaction Importer\RAM-Reaction-Importer\beamData.txt";
            string beamDataString = "";
            Char lineDelimiter = ';';
            Char propertyDelimiter = ',';

            using (StreamReader sr = new StreamReader(path))
            {
                // Read the stream to a string.
                beamDataString = sr.ReadToEnd();
            }
            String[] allBeamData = beamDataString.Split(lineDelimiter);
            allBeamData = allBeamData.Take(allBeamData.Length - 1).ToArray();
            foreach (var singleBeamData in allBeamData)
            {
                bool isCantilevered = false;
                string[] beamProperties = singleBeamData.Split(propertyDelimiter);
                    if (beamProperties[5] == "NA")
                    {
                        beamProperties[5] = "0";
                    }
                    if (beamProperties[6] == "NA")
                    {
                        beamProperties[6] = "0";
                    }
                    if (beamProperties[8] == "NA")
                    {
                        beamProperties[8] = "0";
                    }
                    if (beamProperties[10] == "NA")
                    {
                        beamProperties[10] = "0";
                        isCantilevered = true;
                    }

                RAMBeam ramBeam = new RAMBeam(beamProperties[0], beamProperties[2], Convert.ToDouble(beamProperties[3])*12.0, Convert.ToDouble(beamProperties[4])*12.0,
                    Convert.ToDouble(beamProperties[5])*12.0, Convert.ToDouble(beamProperties[6])*12.0, Convert.ToDouble(beamProperties[7]), Convert.ToDouble(beamProperties[8]), Convert.ToDouble(beamProperties[9]), Convert.ToDouble(beamProperties[10]));
                ramBeam.IsCantilevered = isCantilevered;
                ramBeam.Id = Int32.Parse(beamProperties[1]);

                // Calculate Total Factored Reactions based on design code critera.
                ApplyLoadFactors(designCode, loadFactors, ramBeam);
                ramModel.RamBeams.Add(ramBeam);
            }
            DeserializeRAMStoryData(ramModel);
            DeserializeRAMGridData(ramModel);
            PopulateAdditionalRAMModelInfo(ramModel);
            return ramModel;
        }

        public static void DeserializeRAMStoryData(RAMModel ramModel)
        {
            string path = @"C:\dev\RAM Reaction Importer\RAM-Reaction-Importer\RAMStoryData.txt";
            string storyDataString = "";
            Char lineDelimiter = ';';
            Char propertyDelimiter = ',';
            using (StreamReader sr = new StreamReader(path))
            {
                // Read the stream to a string.
                storyDataString = sr.ReadToEnd();
            }
            String[] allStoryData = storyDataString.Split(lineDelimiter);
            foreach (var singleStoryData in allStoryData)
            {
                string[] storyProperties = singleStoryData.Split(propertyDelimiter);
                Story ramStory = new Story(Convert.ToInt32(storyProperties[0]), storyProperties[1], storyProperties[2], Convert.ToDouble(storyProperties[3]), Convert.ToDouble(storyProperties[4]));
                ramModel.Stories.Add(ramStory);
            }
        }

        public static void DeserializeRAMGridData(RAMModel ramModel)
        {
            string pathX = @"C:\dev\RAM Reaction Importer\RAM-Reaction-Importer\xGridData.txt";
            string pathY = @"C:\dev\RAM Reaction Importer\RAM-Reaction-Importer\yGridData.txt";

            string xGridDataString = "";
            string yGridDataString = "";
            Char lineDelimiter = ';';
            Char propertyDelimiter = ',';
            using (StreamReader sr = new StreamReader(pathX))
            {
                // Read the stream to a string.
                xGridDataString = sr.ReadToEnd();
            }
            String[] allXGridData = xGridDataString.Split(lineDelimiter);
            foreach (var singleXGridData in allXGridData)
            {
                string[] xGridProperties = singleXGridData.Split(propertyDelimiter);
                RAMGrid ramXGrid = new RAMGrid(xGridProperties[0], Convert.ToDouble(xGridProperties[1])*12.0); // Convert feet to inches.
                ClassifyGridNameType(ramXGrid);
                ramXGrid.GridOrientation = RAMGrid.GridOrientationClassification.Vertical;
                ramModel.VerticalGrids.Add(ramXGrid);
                ramModel.Grids.Add(ramXGrid);
            }

            using (StreamReader sr = new StreamReader(pathY))
            {
                // Read the stream to a string.
                yGridDataString = sr.ReadToEnd();
            }
            String[] allYGridData = yGridDataString.Split(lineDelimiter);
            foreach (var singleYGridData in allYGridData)
            {
                string[] yGridProperties = singleYGridData.Split(propertyDelimiter);
                RAMGrid ramYGrid = new RAMGrid(yGridProperties[0], Convert.ToDouble(yGridProperties[1])*12.0); // Convert feet to inches.
                ClassifyGridNameType(ramYGrid);
                ramYGrid.GridOrientation = RAMGrid.GridOrientationClassification.Horizontal;
                ramModel.HorizontalGrids.Add(ramYGrid);
                ramModel.Grids.Add(ramYGrid);
            }
        }

        public static void ClassifyGridDirectionalities(List<RAMGrid> grids, List<RAMGrid> numberedGrids, List<RAMGrid> letteredGrids)
        {
            foreach (var grid in grids)
            {
                if(grid.GridTypeNaming == RAMGrid.GridTypeNamingClassification.Lettered)
                {
                    letteredGrids.Add(grid);
                }
                else if (grid.GridTypeNaming == RAMGrid.GridTypeNamingClassification.Numbered)
                {
                    numberedGrids.Add(grid);
                }
                else
                {
                    throw new Exception("Grid does not have proper GridTypeClassification");
                }
            }

            SortGridsByAlphabeticalOrder(letteredGrids);
            SortGridsByNuermicalOrder(numberedGrids);
            ClassifyGridDirectionality(letteredGrids);
            ClassifyGridDirectionality(numberedGrids);

        }

        public static void SortGridsByAlphabeticalOrder(List<RAMGrid> letteredGrids)
        {
            letteredGrids.OrderBy(x => x.Name);
        }

        public static void SortGridsByNuermicalOrder(List<RAMGrid> numberedGrids)
        {
            numberedGrids.OrderBy(x => Convert.ToInt32(x.Name));
        }

        public static void ClassifyGridDirectionality(List<RAMGrid> grids)
        {
            if (grids.Count > 1)
            {
                var locationA = grids[0].Location;
                var locationB = grids[1].Location;
                if (locationA < locationB)
                {
                    foreach (var grid in grids)
                    {
                        grid.DirectionalityClassification = GridDirectionalityClassification.Increasing;
                    }
                }
                else if (locationA > locationB)
                {
                    foreach (var grid in grids)
                    {
                        grid.DirectionalityClassification = GridDirectionalityClassification.Decreasing;
                    }
                }
                else
                {
                    throw new Exception("Comparing overlapping Grids error");
                }
            }
            else
            {
                throw new Exception("Only 1 RAM grid in that direction found error");
            }
        }

        public static void ClassifyGridNameType(RAMGrid ramGrid)
        {
            Char gridNameDelimiter = '.';
            string gridName = ramGrid.Name;
            bool gridNameContainsPeriod = gridName.Contains(gridNameDelimiter);
            if(gridNameContainsPeriod)
            {
                string[] gridNameComponents = gridName.Split(gridNameDelimiter);
                int n;
                bool isNumeric = int.TryParse(gridNameComponents[0], out n);
                if (isNumeric)
                {
                    ramGrid.GridTypeNaming = RAMGrid.GridTypeNamingClassification.Numbered;
                }
                else
                {
                    ramGrid.GridTypeNaming = RAMGrid.GridTypeNamingClassification.Lettered;
                }
            }
            else
            {
                int n;
                bool isNumeric = int.TryParse(gridName, out n);
                if(isNumeric)
                {
                    ramGrid.GridTypeNaming = RAMGrid.GridTypeNamingClassification.Numbered;
                }
                else
                {
                    ramGrid.GridTypeNaming = RAMGrid.GridTypeNamingClassification.Lettered;
                }
            }

        }

        // STUDS
        public List<RAMBeam> MapStudCountsToRAMBeams(Dictionary<string, Dictionary<int, int>> layoutTypeToBeamStudCount, List<RAMBeam> ramBeams)
        {
            // Loop over RAMBeamList and find matching beams in the stud file parser
            // beamList and assign the stud count.
            for (int j = 0; j < layoutTypeToBeamStudCount.Keys.Count; j++)
            {
                var studFileBeamLayoutType = layoutTypeToBeamStudCount.Keys.ToList()[j];

                for (int i = 0; i < ramBeams.Count; i++)
                {
                    var ramBeamFloorLayoutTypeUnStripped = ramBeams[i].FloorLayoutType;
                    string ramBeamFloorLayoutType = ramBeamFloorLayoutTypeUnStripped.Substring(12, ramBeamFloorLayoutTypeUnStripped.Length - 12);

                    if (studFileBeamLayoutType == ramBeamFloorLayoutType)
                    {
                        var ramBeamId = ramBeams[i].Id;
                        Dictionary<int, int> studFileBeamIdToStudCountDict = layoutTypeToBeamStudCount.Values.ToList()[j];

                        for (int k = 0; k < studFileBeamIdToStudCountDict.Keys.Count; k++)
                        {
                            int studFileBeamId = studFileBeamIdToStudCountDict.Keys.ToList()[k];
                            if(studFileBeamId == ramBeamId)
                            {
                                int studFileBeamStudCount = studFileBeamIdToStudCountDict.Values.ToList()[k];
                                RamBeams[i].StudCount = studFileBeamStudCount;
                            }

                        }

                    }
                    else
                    {
                        continue;
                    }

                }
            }

            return RamBeams;
        }

        public Dictionary<string, Dictionary<int, int>> ParseStudFile(string ramStudsFilePath)
        {
            int lastRowIndex = 0;
            var layoutTypeRowIndexes = new Dictionary<string, int>();
            var layoutTypeToBeamIds = new Dictionary<string, List<int>>();
            var layoutTypeToBeamStudCount = new Dictionary<string, Dictionary<int, int>>();
            var beamIdList = new List<string>();
            var studCountList = new List<string>();

            using (StreamReader sr = new StreamReader(ramStudsFilePath))
            {
                String line;
                int rowIndex = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    rowIndex += 1;
                    string[] parts = line.Split(',');

                    string beamIds = parts[0];
                    if (parts.Length > 8)
                    {
                        string studCount = parts[9];
                        studCountList.Add(studCount);
                    }
                    else
                    {
                        studCountList.Add(" ");
                    }

                    beamIdList.Add(beamIds);
                    if (beamIds.Contains("Floor Type:"))
                    {
                        string layoutType = beamIds.Substring(12, beamIds.Length - 12);
                        layoutTypeRowIndexes.Add(layoutType, rowIndex);
                    }
                    if (beamIds.Contains("*"))
                    {
                        lastRowIndex = rowIndex;
                    }
                }
            }

            var listOfEnds = new List<int[]>();
            for (int j = 0; j < layoutTypeRowIndexes.Keys.Count - 1; j++)
            {
                var ends = new int[2];
                ends[0] = layoutTypeRowIndexes.Values.ToList()[j];
                ends[1] = layoutTypeRowIndexes.Values.ToList()[j + 1];
                listOfEnds.Add(ends);
            }
            var lastEnds = new int[2];
            lastEnds[0] = layoutTypeRowIndexes.Values.ToList().Last();
            lastEnds[1] = lastRowIndex;
            listOfEnds.Add(lastEnds);

            for (int i = 0; i < listOfEnds.Count; i++)
            {
                Dictionary<int, int> beamIdToStudCount = new Dictionary<int, int>();
                var beamsPerFLoor = beamIdList.GetRange(listOfEnds[i][0] + 2, listOfEnds[i][1] - listOfEnds[i][0] - 3);
                var studCountPerFloor = studCountList.GetRange(listOfEnds[i][0] + 2, listOfEnds[i][1] - listOfEnds[i][0] - 3);
                for (int k = 0; k < beamsPerFLoor.Count; k++)
                {
                    if (String.IsNullOrWhiteSpace(beamsPerFLoor[k]))
                    {
                        beamsPerFLoor.RemoveAt(k);
                        studCountPerFloor.RemoveAt(k);
                    }
                }
                var intBeamsPerFloor = beamsPerFLoor.Select(int.Parse).ToList();
                var intStudCountPerFloor = new List<int>();

                layoutTypeToBeamIds.Add(layoutTypeRowIndexes.Keys.ToList()[i], intBeamsPerFloor);

                for (int s = 0; s < intBeamsPerFloor.Count; s++)
                {
                    if (studCountPerFloor[s].Contains("Studs") || String.IsNullOrWhiteSpace(studCountPerFloor[s]) || studCountPerFloor[s] == "")
                    {
                        intStudCountPerFloor.Add(0);
                    }
                    else
                    {
                        intStudCountPerFloor.Add(Int32.Parse(studCountPerFloor[s]));
                    }
                    beamIdToStudCount.Add(intBeamsPerFloor[s], intStudCountPerFloor[s]);
                }
                layoutTypeToBeamStudCount.Add(layoutTypeRowIndexes.Keys.ToList()[i], beamIdToStudCount);
            }
            return layoutTypeToBeamStudCount;
        }

        // CAMBER
        public class CamberParser
        {
            public class BeamCamberFile
            {
                public string FloorLayoutType { get; set; }
                public int Id { get; set; }
                public double Camber { get; set; }
                public bool IsComposite { get; set; }

            }

            public static List<BeamCamberFile> ParseCamberFile(string ramCamberFilePath)
            {
                var beamList = new List<BeamCamberFile>();
                bool IsComposite = false;
                string layoutType = "";
                var camberList = new List<string>();

                using (StreamReader sr = new StreamReader(ramCamberFilePath))
                {
                    String line;
                    int rowIndex = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        rowIndex += 1;
                        string[] parts = line.Split(',');

                        string firstColumn = parts[0];
                        if (firstColumn.Contains("Floor Type:"))
                        {
                            layoutType = firstColumn.Substring(13, firstColumn.Length - 13);
                        }
                        if (firstColumn.Contains("Noncomposite"))
                        {
                            IsComposite = false;
                        }
                        else if (firstColumn.Contains("Composite"))
                        {
                            IsComposite = true;
                        }
                        int beamId;
                        if (int.TryParse(firstColumn, out beamId))
                        {
                            BeamCamberFile beam = new BeamCamberFile();
                            beam.FloorLayoutType = layoutType;
                            beam.Id = beamId;
                            beam.IsComposite = IsComposite;
                            string camber = "";
                            double camberDouble = 0.0;
                            if (!IsComposite)
                            {
                                try
                                {
                                    camber = parts[5];
                                    camberList.Add(camber);
                                }
                                catch
                                {
                                    Array.Resize(ref parts, 6);
                                    parts[6 - 1] = " ";
                                    camber = parts[5];
                                    camberList.Add(camber);
                                }
                            }
                            else if (IsComposite)
                            {
                                try
                                {
                                    camber = parts[6];
                                    camberList.Add(camber);
                                }
                                catch
                                {
                                    Array.Resize(ref parts, 7);
                                    parts[7 - 1] = " ";
                                    camber = parts[6];
                                    camberList.Add(camber);
                                }
                            }
                            if (double.TryParse(camber, out camberDouble))
                            {
                                beam.Camber = camberDouble;
                            }
                            else
                            {
                                beam.Camber = 0.0;
                            }

                            beamList.Add(beam);
                        }
                    }
                }
                return beamList;
            }
        }

        public List<RAMBeam> MapCamberToRAMBeams(List<CamberParser.BeamCamberFile> beamsFromCamberFile, List<RAMBeam> ramBeams)
        {
            for(int i=0; i < ramBeams.Count; i++)
            {
                var ramBeamFloorLayoutTypeUnStripped = ramBeams[i].FloorLayoutType;
                string ramBeamFloorLayoutType = ramBeamFloorLayoutTypeUnStripped.Substring(12, ramBeamFloorLayoutTypeUnStripped.Length - 12);
                var ramBeamId = ramBeams[i].Id;
                for (int j = 0; j < beamsFromCamberFile.Count; j++)
                {
                    var layoutTypeFromCamberFileUnTrimmed = beamsFromCamberFile[j].FloorLayoutType;
                    string layoutTypeFromCamberFile = layoutTypeFromCamberFileUnTrimmed.Trim();
                    if (layoutTypeFromCamberFile == ramBeamFloorLayoutType)
                    {
                        var beamIdFromCamberFile = beamsFromCamberFile[j].Id;
                        if (beamIdFromCamberFile == ramBeamId)
                        {
                            ramBeams[i].Camber = beamsFromCamberFile[j].Camber;
                        }
                    }
                }
            }
            return ramBeams;
        }
    }

    public enum DesignCode
    {
        LRFD,
        ASD
    }
}
