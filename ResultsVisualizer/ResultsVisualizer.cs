﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitReactionImporter
{
    public enum AnnotationType
    {
        None,
        Reaction,
        StudCount,
        Camber,
        Size
    }

    public class ResultsAnnotator
    {
        private Document _document = null;
        private ModelCompare.Results Results { get; set; }

        

        public ResultsAnnotator(Document document, ModelCompare.Results results, AnnotationType annotationType)
        {
            Results = results;

            _document = document;
        }

        public static void AnnotateBeams(Document document, ModelCompare.Results results, AnnotationType annotationType)
        {
            if(annotationType == AnnotationType.Reaction)
            {
                AnnotateReactions(document, results);
            }
            else if (annotationType == AnnotationType.StudCount)
            {
                AnnotateStudCounts(document, results);
            }
            else if (annotationType == AnnotationType.Camber)
            {
                AnnotateCamberValues(document, results);
            }
            else if (annotationType == AnnotationType.Size)
            {
                return;
                // TODO:
                //ChangeSize(document, results);
            }
            else
            {
                throw new Exception("Need to debug: No Annotation Type has been defined.");
            }
        }

        public static void AnnotateReactions(Document document, ModelCompare.Results results)
        {
            var revitBeamToRAMBeamMappingDict = results.RevitBeamToRAMBeamMapping;
            var annotateReactionsTransaction = new Transaction(document, "Annotate Beam Reactions");
            annotateReactionsTransaction.Start();
            foreach (var revitBeam in revitBeamToRAMBeamMappingDict.Keys)
            {
                revitBeam.StartReactionTotal = revitBeamToRAMBeamMappingDict[revitBeam].StartTotalReactionPositive.ToString();
                revitBeam.EndReactionTotal = revitBeamToRAMBeamMappingDict[revitBeam].EndTotalReactionPositive.ToString();
                ElementId beamId = new ElementId(revitBeam.ElementId);
                var revitBeamInstance = document.GetElement(beamId) as FamilyInstance;
                var startReactionTotalParameter = revitBeamInstance.LookupParameter("Start Reaction - Total");
                var endReactionTotalParameter = revitBeamInstance.LookupParameter("End Reaction - Total");
                startReactionTotalParameter.SetValueString(revitBeam.StartReactionTotal);
                endReactionTotalParameter.SetValueString(revitBeam.EndReactionTotal);
            }
            annotateReactionsTransaction.Commit();
        }

        public static void AnnotateStudCounts(Document document, ModelCompare.Results results)
        {
            var revitBeamToRAMBeamMappingDict = results.RevitBeamToRAMBeamMapping;
            var annotateStudCountsTransaction = new Transaction(document, "Annotate Beam Stud Counts");
            annotateStudCountsTransaction.Start();
            foreach (var revitBeam in revitBeamToRAMBeamMappingDict.Keys)
            {
                revitBeam.StudCount = revitBeamToRAMBeamMappingDict[revitBeam].StudCount.ToString();
                ElementId beamId = new ElementId(revitBeam.ElementId);
                var revitBeamInstance = document.GetElement(beamId) as FamilyInstance;
                var studCountParameter = revitBeamInstance.LookupParameter("Number of studs");
                studCountParameter.Set(revitBeam.StudCount);//.ToString();
            }
            annotateStudCountsTransaction.Commit();

        }

        public static void AnnotateCamberValues(Document document, ModelCompare.Results results)
        {
            var revitBeamToRAMBeamMappingDict = results.RevitBeamToRAMBeamMapping;
            var annotateCamberValuesTransaction = new Transaction(document, "Annotate Beam Camber Values");
            annotateCamberValuesTransaction.Start();
            foreach (var revitBeam in revitBeamToRAMBeamMappingDict.Keys)
            {
                revitBeam.Camber = revitBeamToRAMBeamMappingDict[revitBeam].Camber.ToString();
                ElementId beamId = new ElementId(revitBeam.ElementId);
                var revitBeamInstance = document.GetElement(beamId) as FamilyInstance;
                var camberParameter = revitBeamInstance.LookupParameter("Camber Size");
                camberParameter.Set(revitBeam.Camber).ToString();
            }
            annotateCamberValuesTransaction.Commit();

        }

        // TODO:
        public static void ChangeSize(Document document, ModelCompare.Results results)
        {
            var beamsToAnnotate = results.MappedRevitBeams;
            foreach (var beamToAnnotate in beamsToAnnotate)
            {
                ElementId beamId = new ElementId(beamToAnnotate.ElementId);
                var revitBeam = document.GetElement(beamId) as FamilyInstance;
                var revitBeamSize = beamToAnnotate.Size;
                revitBeamSize = revitBeamSize.Replace(" ", "");
                var beamToAnnotateRAMSize = beamToAnnotate.RAMSize;
                if(revitBeamSize != beamToAnnotateRAMSize)
                {
                    //revitBeam.Symbol = new FamilySymbol()
                }
                else
                {
                    continue;
                }


            }
        }





    }

        public class ResultsVisualizer
    {
        private Document _document = null;

        public Dictionary<ColorMapCategories, Color> ColorMap { get; set; }

        public ResultsVisualizer(Document document)
        {
            _document = document;

            ColorMap = new Dictionary<ColorMapCategories, Color>();
            ColorMap.Add(ColorMapCategories.Null, new Color(210, 0, 0)); // RED
            ColorMap.Add(ColorMapCategories.UserInput, new Color(0, 0, 230)); // BLUE
            ColorMap.Add(ColorMapCategories.RAMImport, new Color(0, 190, 0)); // GREEN

            ColorMap.Add(ColorMapCategories.SameSizeBeam, new Color(0, 190, 0)); // GREEN
            ColorMap.Add(ColorMapCategories.DifferentSizeBeam, new Color(255, 128, 0)); // ORANGE

        }


        public enum ColorMapCategories
        {
            Null,
            RAMImport,
            UserInput,
            SameSizeBeam,
            DifferentSizeBeam
        }

        public List<Beam> GetUnMappedBeams(List<Beam> mappedRevitBeams, List<Beam> modelBeams)
        {
            var unMappedBeams = new List<Beam>();
            foreach(var modelBeam in modelBeams)
            {
                if(!modelBeam.IsMappedToRAMBeam)
                {
                    unMappedBeams.Add(modelBeam);
                }

            }
            return unMappedBeams;
        }


        // REACTIONS.
        public void ColorMembers(AnalyticalModel model, string annotationToVisualize, List<Beam> mappedRevitBeams, List<Beam> modelBeams)
        {
            var unMappedBeams = GetUnMappedBeams(mappedRevitBeams, modelBeams);
            var nullBeams = new List<Beam>();
            var userInputBeams = new List<Beam>();
            var wrongSizedBeams = new List<Beam>();
            var rightSizedBeams = new List<Beam>();

            ChooseAnnotationToVisualize(unMappedBeams, nullBeams, userInputBeams, annotationToVisualize, wrongSizedBeams, rightSizedBeams, mappedRevitBeams);

            var projectPatterns = new FilteredElementCollector(_document);
            projectPatterns.OfClass(typeof(FillPatternElement));
            var patternIds = (IList<ElementId>)projectPatterns.ToElementIds();
            var ogs = new OverrideGraphicSettings();
            ogs.SetProjectionLineWeight(10);
            if (annotationToVisualize == "VisualizeRAMSizes")
            {
                // Color Un-mapped Beams in Red.
                var colorNullElem = new Transaction(_document, "Color Unmapped Beams");
                colorNullElem.Start();
                Color nullColor = ColorMap[ColorMapCategories.Null];
                foreach (var beam in unMappedBeams)
                {
                    ogs.SetProjectionLineColor(nullColor);
                    _document.ActiveView.SetElementOverrides(new ElementId(beam.ElementId), ogs);
                }
                colorNullElem.Commit();

                // Color Beams in Revit that were mapped with RAM.
                var colorRAMImportElem = new Transaction(_document, "Color RAM Import Beams");
                colorRAMImportElem.Start();
                Color sameSizedBeamColor = ColorMap[ColorMapCategories.SameSizeBeam];
                Color differentSizedBeamColor = ColorMap[ColorMapCategories.DifferentSizeBeam];

                foreach (var beam in rightSizedBeams)
                {
                    ogs.SetProjectionLineColor(sameSizedBeamColor);
                    _document.ActiveView.SetElementOverrides(new ElementId(beam.ElementId), ogs);
                }
                foreach (var beam in wrongSizedBeams)
                {
                    ogs.SetProjectionLineColor(differentSizedBeamColor);
                    _document.ActiveView.SetElementOverrides(new ElementId(beam.ElementId), ogs);
                }
                colorRAMImportElem.Commit();
            }
            else
            {
                var colorNullElem = new Transaction(_document, "Color Null Beams");
                colorNullElem.Start();
                Color nullColor = ColorMap[ColorMapCategories.Null];
                foreach (var beam in nullBeams)
                {
                    ogs.SetProjectionLineColor(nullColor);
                    _document.ActiveView.SetElementOverrides(new ElementId(beam.ElementId), ogs);
                }
                colorNullElem.Commit();

                // Color Beams in Revit that were mapped with RAM.
                var colorRAMImportElem = new Transaction(_document, "Color RAM Import Beams");
                colorRAMImportElem.Start();
                Color ramImportColor = ColorMap[ColorMapCategories.RAMImport];
                foreach (var beam in mappedRevitBeams)
                {
                    ogs.SetProjectionLineColor(ramImportColor);
                    _document.ActiveView.SetElementOverrides(new ElementId(beam.ElementId), ogs);
                }
                colorRAMImportElem.Commit();
            }

        }

        public void ChooseAnnotationToVisualize(List<Beam> unMappedBeams, List<Beam> nullBeams, List<Beam> userInputBeams, string annotationToVisualize, List<Beam> wrongSizedBeams, List<Beam> rightSizedBeams, List<Beam> mappedBeams)
        {
            if (annotationToVisualize == "VisualizeRAMReactions")
            {
                CategorizeReactionValues(unMappedBeams, nullBeams, userInputBeams);
            }
            if (annotationToVisualize == "VisualizeRAMSizes")
            {
                CategorizeSizeValues(wrongSizedBeams, rightSizedBeams, mappedBeams);
            }
            if (annotationToVisualize == "VisualizeRAMStuds")
            {
                CategorizeStudCountValues(unMappedBeams, nullBeams, userInputBeams);

            }
            if (annotationToVisualize == "VisualizeRAMCamber")
            {
                CategorizeCamberValues(unMappedBeams, nullBeams, userInputBeams);
            }
        }


        // REACTIONS.
        public void CategorizeReactionValues(List<Beam> unMappedBeams, List<Beam> nullBeams, List<Beam> userInputBeams)
        {
            foreach (var beam in unMappedBeams)
            {
                ElementId beamId = new ElementId(beam.ElementId);
                var revitBeam = _document.GetElement(beamId) as FamilyInstance;
                var startReactionTotalParameter = revitBeam.LookupParameter("Start Reaction - Total");
                var endReactionTotalParameter = revitBeam.LookupParameter("End Reaction - Total");

                bool startRxnParameterHasValue = startReactionTotalParameter.HasValue;
                bool endRxnParameterHasValue = endReactionTotalParameter.HasValue;

                //var startReactionTotal = revitBeam.LookupParameter("Start Reaction - Total").AsString() != null ? revitBeam.LookupParameter("Start Reaction - Total").AsValueString() : "";
                //var endReactionTotal = revitBeam.LookupParameter("End Reaction - Total").AsString() != null ? revitBeam.LookupParameter("End Reaction - Total").AsValueString() : "";

                if (!startRxnParameterHasValue || !endRxnParameterHasValue)
                    {
                        nullBeams.Add(beam);
                    }
                    else
                    {
                        userInputBeams.Add(beam);
                    }
            }
        }


        // STUDS.
        public void CategorizeStudCountValues(List<Beam> unMappedBeams, List<Beam> nullBeams, List<Beam> userInputBeams)
        {
            foreach (var beam in unMappedBeams)
            {
                ElementId beamId = new ElementId(beam.ElementId);
                var revitBeam = _document.GetElement(beamId) as FamilyInstance;
                var studCount = revitBeam.LookupParameter("Number of studs");

                if (!studCount.HasValue)
                {
                    nullBeams.Add(beam);
                }
                else
                {
                    userInputBeams.Add(beam);
                }
            }
        }

        // CAMBER.
        public void CategorizeCamberValues(List<Beam> unMappedBeams, List<Beam> nullBeams, List<Beam> userInputBeams)
        {
            foreach (var beam in unMappedBeams)
            {
                ElementId beamId = new ElementId(beam.ElementId);
                var revitBeam = _document.GetElement(beamId) as FamilyInstance;
                var camberSize = revitBeam.LookupParameter("Camber Size");

                if (!camberSize.HasValue)
                {
                    nullBeams.Add(beam);
                }
                else
                {
                    userInputBeams.Add(beam);
                }
            }
        }

        // SIZES.
        public void CategorizeSizeValues(List<Beam> wrongSizedBeams, List<Beam> rightSizedBeams, List<Beam> mappedBeams)
        {
            foreach (var revitBeam in mappedBeams)
            {
                //ElementId beamId = new ElementId(beam.ElementId);
                //var revitBeam = _document.GetElement(beamId) as FamilyInstance;
                var revitBeamSize = revitBeam.Size;
                var ramBeamSize = revitBeam.RAMSize;

                if (revitBeamSize == ramBeamSize)
                {
                    rightSizedBeams.Add(revitBeam);
                }
                else
                {
                    rightSizedBeams.Add(revitBeam);
                }
            }


        }
        public void ResetVisualsInActiveView(List<Beam> modelBeams)
        {
            var ogs = new OverrideGraphicSettings();

            foreach (var elem in modelBeams)
            {
                var reset = new Transaction(_document, "Reset One Element");
                reset.Start();
                _document.ActiveView.SetElementOverrides(new ElementId(elem.ElementId), ogs);
                //_document.ActiveView.DisplayStyle = DisplayStyle.Shading;
                reset.Commit();
            }
        }

        // CLEAR DATA.
        public void ClearSelectedAnnotations()
        {

        }


        }
}
