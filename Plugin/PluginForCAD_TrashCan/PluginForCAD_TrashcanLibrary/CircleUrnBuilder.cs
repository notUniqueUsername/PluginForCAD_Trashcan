using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kompas6API5;

namespace PluginForCAD_TrashcanLibrary
{
    public class CircleUrnBuilder
    {
        /// <summary>
        /// Экземпляр компаса
        /// </summary>
        private KompasObject _kompas;

        /// <summary>
        /// Интерфеис для хранения сущностей(эскиз, операции и прочее)
        /// </summary>
        private ksEntity _entity;

        /// <summary>
        /// Деталь
        /// </summary>
        private ksPart _part;

        /// <summary>
        /// Струтура параметров 2д окружности
        /// </summary>
        private ksCircleParam _circleParam;

        /// <summary>
        /// Струтура параметров 2д прямоугольника
        /// </summary>
        private ksRectangleParam _rectangleParam;

        /// <summary>
        /// Струтура параметров 2д отрезка
        /// </summary>
        private ksLineSegParam _lineParam;

        /// <summary>
        /// Документ 
        /// </summary>
        private Document3D _doc3D;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="kompas"></param>
        public CircleUrnBuilder(KompasObject kompas)
        {
            _kompas = kompas;
        }

        /// <summary>
        /// Строим пепельницу
        /// </summary>
        /// <param name="parameters"></param>
        private void AshtrayBuild(CircleParameters parameters)
        {
            const double ashtrayHihght = 30;
            const double ashtrayThickness = 5;
            const double ashtrayRadius = 30;
            #region Смещение плоскости для основания пепельницы
            _part = _doc3D.GetPart((short)KSConstants.pTop_part);
            ksEntity entityAshtrayPlane = _part.GetDefaultEntity((short)KSConstants.o3d_planeXOY);

            ksEntity ashtrayPlane = _part.NewEntity((short)KSConstants.o3d_planeOffset);
            ksPlaneOffsetDefinition ashtrayPlaneOffsetDefinition = ashtrayPlane.GetDefinition();
            ashtrayPlaneOffsetDefinition.direction = true;
            ashtrayPlaneOffsetDefinition.offset = ashtrayHihght;
            ashtrayPlaneOffsetDefinition.SetPlane(entityAshtrayPlane);
            ashtrayPlane.Create();
            #endregion

            #region Смещение плоскости для основания толщины пепельницы
            _part = _doc3D.GetPart((short)KSConstants.pTop_part);
            ksEntity entityAshtrayThicknessPlane = _part.GetDefaultEntity((short)KSConstants.o3d_planeXOY);

            ksEntity ashtrayThicknessPlane = _part.NewEntity((short)KSConstants.o3d_planeOffset);
            ksPlaneOffsetDefinition ashtrayThicknessPlaneOffsetDefinition = ashtrayThicknessPlane.GetDefinition();
            ashtrayThicknessPlaneOffsetDefinition.direction = true;
            ashtrayThicknessPlaneOffsetDefinition.offset = ashtrayHihght - ashtrayThickness;
            ashtrayThicknessPlaneOffsetDefinition.SetPlane(entityAshtrayThicknessPlane);
            ashtrayThicknessPlane.Create();
            #endregion

            #region Эскиз основания пепельницы

            _part = _doc3D.GetPart((short)KSConstants.pTop_part);
            ksEntity ashtrayEntity = _part.NewEntity((short)KSConstants.o3d_sketch);
            ksSketchDefinition ashtraySketchDefinition = ashtrayEntity.GetDefinition();
            ashtraySketchDefinition.SetPlane(ashtrayPlane);
            ashtrayEntity.Create();
            ksDocument2D document2DAshtray = ashtraySketchDefinition.BeginEdit();
            _lineParam = _kompas.GetParamStruct((short)KSConstants.ko_LineSegParam);
            _lineParam.y1 = -ashtrayRadius;
            _lineParam.x1 = parameters.RadiusBottom  - parameters.WallThickness * 0.75;
            _lineParam.x2 = _lineParam.x1;
            _lineParam.y2 = ashtrayRadius;
            _lineParam.style = 1;
            document2DAshtray.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _lineParam.style);

            var arcLeftPointX = _lineParam.x1;
            var arcLeftPointY = _lineParam.y1;
            var arcRightPointX = _lineParam.x2;
            var arcRightPointY = _lineParam.y2;
            var arcMidlePointX = _lineParam.x1 - ashtrayRadius;
            var arcMidlePointY = 0;

            document2DAshtray.ksArcBy3Points(arcLeftPointX, arcLeftPointY, arcMidlePointX, arcMidlePointY, arcRightPointX, arcRightPointY, 1);
            ashtraySketchDefinition.EndEdit();
            #endregion

            #region Эскиз верха основания пепельницы

            _part = _doc3D.GetPart((short)KSConstants.pTop_part);
            ksEntity ashtrayTopEntity = _part.NewEntity((short)KSConstants.o3d_sketch);
            ksSketchDefinition ashtrayTopSketchDefinition = ashtrayTopEntity.GetDefinition();
            ashtrayTopSketchDefinition.SetPlane(ashtrayThicknessPlane);
            ashtrayTopEntity.Create();
            ksDocument2D document2DAshtrayTop = ashtrayTopSketchDefinition.BeginEdit();
            _lineParam = _kompas.GetParamStruct((short)KSConstants.ko_LineSegParam);
            _lineParam.y1 = -ashtrayRadius + ashtrayThickness;
            _lineParam.x1 = parameters.RadiusBottom - parameters.WallThickness *0.75;
            _lineParam.x2 = _lineParam.x1;
            _lineParam.y2 = ashtrayRadius - ashtrayThickness;
            _lineParam.style = 1;
            document2DAshtrayTop.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _lineParam.style);

            arcLeftPointX = _lineParam.x1;
            arcLeftPointY = _lineParam.y1;
            arcRightPointX = _lineParam.x2;
            arcRightPointY = _lineParam.y2;
            arcMidlePointX = _lineParam.x1 - ashtrayRadius + ashtrayThickness;
            arcMidlePointY = 0 - 5;

            document2DAshtrayTop.ksArcBy3Points(arcLeftPointX, arcLeftPointY, arcMidlePointX, arcMidlePointY, arcRightPointX, arcRightPointY, 1);
            ashtrayTopSketchDefinition.EndEdit();
            #endregion

            #region Выдавливание пепельницы
            const short etBlind = 0;
            const short dtNormal = 0;
            const short dtReverse = 1;
            ksEntity entityBaseExtrusionAshtray = _part.NewEntity((short)KSConstants.o3d_bossExtrusion);
            ksBossExtrusionDefinition extrusionDefinitionAshtray = entityBaseExtrusionAshtray.GetDefinition();
            extrusionDefinitionAshtray.directionType = dtReverse;
            extrusionDefinitionAshtray.SetSideParam(false, etBlind, ashtrayHihght, 0, false);
            extrusionDefinitionAshtray.SetSketch(ashtraySketchDefinition);
            entityBaseExtrusionAshtray.Create();
            #endregion

            #region Вырезание полости пепельницы
            ksEntity entityCutExtrusionAshtray = _part.NewEntity((short)KSConstants.o3d_cutExtrusion);
            ksCutExtrusionDefinition cutExtrusionDefinitionAshtray = entityCutExtrusionAshtray.GetDefinition();
            cutExtrusionDefinitionAshtray.cut = true;
            cutExtrusionDefinitionAshtray.directionType = dtNormal;
            cutExtrusionDefinitionAshtray.SetSideParam(true, etBlind, ashtrayHihght - ashtrayThickness, 0, false);
            cutExtrusionDefinitionAshtray.SetSketch(ashtrayTopSketchDefinition);
            entityCutExtrusionAshtray.Create();
            #endregion

            #region Эскиз отверстий основания пепельницы

            _part = _doc3D.GetPart((short)KSConstants.pTop_part);
            ksEntity ashtrayHoleEntity = _part.NewEntity((short)KSConstants.o3d_sketch);
            ksSketchDefinition ashtrayHoleSketchDefinition = ashtrayHoleEntity.GetDefinition();
            ashtrayHoleSketchDefinition.SetPlane(ashtrayThicknessPlane);
            ashtrayHoleEntity.Create();
            ksDocument2D document2DAshtrayHole = ashtrayHoleSketchDefinition.BeginEdit();
            _circleParam = _kompas.GetParamStruct((short)KSConstants.ko_CircleParam);
            _circleParam.xc = ((parameters.RadiusBottom - parameters.WallThickness / 2) + (parameters.RadiusBottom - parameters.WallThickness / 2 - ashtrayRadius + ashtrayThickness)) / 2;
            _circleParam.yc = 0;
            _circleParam.rad = 0.5;
            _circleParam.style = 1;
            document2DAshtrayHole.ksCircle(_circleParam.xc, _circleParam.yc, _circleParam.rad, _circleParam.style);

            _circleParam.yc = 5;
            _circleParam.style = 1;
            document2DAshtrayHole.ksCircle(_circleParam.xc, _circleParam.yc, _circleParam.rad, _circleParam.style);

            _circleParam.yc = -5;
            _circleParam.style = 1;
            document2DAshtrayHole.ksCircle(_circleParam.xc, _circleParam.yc, _circleParam.rad, _circleParam.style);

            ashtrayHoleSketchDefinition.EndEdit();
            #endregion

            #region Вырезание отверстий
            ksEntity entityCutExtrusionAshtrayHole = _part.NewEntity((short)KSConstants.o3d_cutExtrusion);
            ksCutExtrusionDefinition cutExtrusionDefinitionAshtrayHole = entityCutExtrusionAshtrayHole.GetDefinition();
            cutExtrusionDefinitionAshtrayHole.cut = true;
            cutExtrusionDefinitionAshtrayHole.directionType = dtReverse;
            cutExtrusionDefinitionAshtrayHole.SetSideParam(true, etBlind, ashtrayThickness, 0, false);
            cutExtrusionDefinitionAshtrayHole.SetSketch(ashtrayHoleSketchDefinition);
            entityCutExtrusionAshtrayHole.Create();
            #endregion

        }

        /// <summary>
        /// Строим урну
        /// </summary>
        /// <param name="parameters"></param>
        public void Build(CircleParameters parameters)
        {
            _doc3D = _kompas.Document3D();
            _doc3D.Create(false, true);
            
            #region Эскиз нижненго основания

            _part = _doc3D.GetPart((short)KSConstants.pTop_part);
            _entity = _part.NewEntity((short)KSConstants.o3d_sketch);
            ksSketchDefinition sketchDefinition = _entity.GetDefinition();
            ksEntity xoyPlane = _part.GetDefaultEntity((short)KSConstants.o3d_planeXOY);
            sketchDefinition.SetPlane(xoyPlane);
            _entity.Create();
            ksDocument2D document2D = sketchDefinition.BeginEdit();

            _circleParam = _kompas.GetParamStruct((short)KSConstants.ko_CircleParam);
            _circleParam.xc = 0;
            _circleParam.yc = 0;
            _circleParam.rad = parameters.RadiusBottom;
            _circleParam.style = 1;
            document2D.ksCircle(_circleParam.xc, _circleParam.yc, _circleParam.rad, _circleParam.style);
            sketchDefinition.EndEdit();
            #endregion

            #region Смещение плоскости для верхнего основания
            _part = _doc3D.GetPart((short)KSConstants.pTop_part);
            ksEntity entityDisplacedPlane = _part.GetDefaultEntity((short)KSConstants.o3d_planeXOY);

            ksEntity displacedPlane = _part.NewEntity((short)KSConstants.o3d_planeOffset);
            ksPlaneOffsetDefinition planeOffsetDefinition = displacedPlane.GetDefinition();
            planeOffsetDefinition.direction = true;
            planeOffsetDefinition.offset = parameters.UrnHeight;
            planeOffsetDefinition.SetPlane(entityDisplacedPlane);
            displacedPlane.Create();
            #endregion

            #region Эскиз верхнегого основания
            ksEntity entityTop = _part.NewEntity((short)KSConstants.o3d_sketch);
            ksSketchDefinition sketchDefinitionTop = entityTop.GetDefinition();
            sketchDefinitionTop.SetPlane(displacedPlane);
            entityTop.Create();
            ksDocument2D document2DTop = sketchDefinitionTop.BeginEdit();
            _circleParam = _kompas.GetParamStruct((short)KSConstants.ko_CircleParam);
            _circleParam.xc = 0;
            _circleParam.yc = 0;
            _circleParam.rad = parameters.RadiusTop;
            _circleParam.style = 1;
            document2DTop.ksCircle(_circleParam.xc, _circleParam.yc, _circleParam.rad, _circleParam.style);
            sketchDefinitionTop.EndEdit();
            #endregion

            #region Операция выдавливания по сечениям
            ksEntity entityLoft = _part.NewEntity((short)KSConstants.o3d_baseLoft);
            ksBaseLoftDefinition loftDefinition = entityLoft.GetDefinition();
            loftDefinition.SetLoftParam(false, true, true);
            ksEntityCollection entityCollection = loftDefinition.Sketchs();
            entityCollection.Clear();
            entityCollection.Add(sketchDefinition);
            entityCollection.Add(sketchDefinitionTop);
            entityLoft.Create();
            #endregion

            #region Смещение плоскости для вырезания верхнего основания
            _part = _doc3D.GetPart((short)KSConstants.pTop_part);
            ksEntity entityDisplacedPlaneTopCut = _part.GetDefaultEntity((short)KSConstants.o3d_planeXOY);

            ksEntity displacedPlaneTopCut = _part.NewEntity((short)KSConstants.o3d_planeOffset);
            ksPlaneOffsetDefinition planeOffsetDefinitionTop = displacedPlaneTopCut.GetDefinition();
            planeOffsetDefinitionTop.direction = true;
            planeOffsetDefinitionTop.offset = 0;
            planeOffsetDefinitionTop.SetPlane(entityDisplacedPlaneTopCut);
            displacedPlaneTopCut.Create();
            #endregion

            #region Смещение плоскости для вырезания нижнего основания
            _part = _doc3D.GetPart((short)KSConstants.pTop_part);
            ksEntity entityDisplacedPlaneBotCut = _part.GetDefaultEntity((short)KSConstants.o3d_planeXOY);

            ksEntity displacedPlaneBotCut = _part.NewEntity((short)KSConstants.o3d_planeOffset);
            ksPlaneOffsetDefinition planeOffsetDefinitionBot = displacedPlaneBotCut.GetDefinition();
            planeOffsetDefinitionBot.direction = true;
            planeOffsetDefinitionBot.offset = parameters.UrnHeight - parameters.BottomThickness;
            planeOffsetDefinitionBot.SetPlane(entityDisplacedPlaneBotCut);
            displacedPlaneBotCut.Create();
            #endregion

            #region Эскиз верхнего основания для вырезания
            ksEntity entityTopCut = _part.NewEntity((short)KSConstants.o3d_sketch);
            ksSketchDefinition sketchTopCut = entityTopCut.GetDefinition();
            sketchTopCut.SetPlane(displacedPlaneTopCut);
            entityTopCut.Create();
            ksDocument2D document2DTopCut = sketchTopCut.BeginEdit();
            _circleParam = _kompas.GetParamStruct((short)KSConstants.ko_CircleParam);
            _circleParam.xc = 0;
            _circleParam.yc = 0;
            _circleParam.rad = parameters.RadiusBottom - parameters.WallThickness;
            _circleParam.style = 1;
            document2DTopCut.ksCircle(_circleParam.xc, _circleParam.yc, _circleParam.rad, _circleParam.style);
            sketchTopCut.EndEdit();
            #endregion

            #region Эскиз нижнего основания для вырезания
            ksEntity entityBotCut = _part.NewEntity((short)KSConstants.o3d_sketch);
            ksSketchDefinition sketchBotCut = entityBotCut.GetDefinition();
            sketchBotCut.SetPlane(displacedPlaneBotCut);
            entityBotCut.Create();
            ksDocument2D document2DBotCut = sketchBotCut.BeginEdit();
            _circleParam = _kompas.GetParamStruct((short)KSConstants.ko_CircleParam);
            _circleParam.xc = 0;
            _circleParam.yc = 0;
            _circleParam.rad = parameters.RadiusTop - parameters.WallThickness;
            _circleParam.style = 1;
            document2DBotCut.ksCircle(_circleParam.xc, _circleParam.yc, _circleParam.rad, _circleParam.style);
            sketchBotCut.EndEdit();
            #endregion

            #region Вырезание по сечениям
            ksEntity entityCutLoft = _part.NewEntity((short)KSConstants.o3d_cutLoft);
            ksCutLoftDefinition cutLoftDefinition = entityCutLoft.GetDefinition();
            cutLoftDefinition.SetLoftParam(false, true, true);
            cutLoftDefinition.cut = true;
            ksEntityCollection entityCollectionCut = cutLoftDefinition.Sketchs();

            entityCollectionCut.Clear();
            entityCollectionCut.Add(sketchBotCut);
            entityCollectionCut.Add(sketchTopCut);
            entityCutLoft.Create();
            #endregion



            //Если есть стойка
            if (parameters.Stand)
            {
                #region Смещеная плоскость для отверстий
                _part = _doc3D.GetPart((short)KSConstants.pTop_part);
                ksEntity entityDisplacedPlaneHoles = _part.GetDefaultEntity((short)KSConstants.o3d_planeXOZ);

                ksEntity displacedPlaneHoles = _part.NewEntity((short)KSConstants.o3d_planeOffset);
                ksPlaneOffsetDefinition planeOffsetDefinitionHoles = displacedPlaneHoles.GetDefinition();
                planeOffsetDefinitionHoles.direction = true;
                planeOffsetDefinitionHoles.offset = parameters.RadiusBottom + 2 * 10;
                planeOffsetDefinitionHoles.SetPlane(entityDisplacedPlaneHoles);
                displacedPlaneHoles.Create();
                #endregion

                #region Эскиз для отверстия
                ksEntity entityHoles = _part.NewEntity((short)KSConstants.o3d_sketch);
                ksSketchDefinition sketchHoles = entityHoles.GetDefinition();
                sketchHoles.SetPlane(displacedPlaneHoles);
                entityHoles.Create();
                ksDocument2D document2DHoles = sketchHoles.BeginEdit();
                _circleParam = _kompas.GetParamStruct((short)KSConstants.ko_CircleParam);
                _circleParam.xc = 0;
                _circleParam.yc = -10 * 10;
                _circleParam.rad = 2.5 * 10;
                _circleParam.style = 1;
                document2DHoles.ksCircle(_circleParam.xc, _circleParam.yc, _circleParam.rad, _circleParam.style);
                sketchHoles.EndEdit();
                #endregion

                #region Отверстия
                const short etBlind = 0;
                const short dtNormal = 0;
                ksEntity entityCutExtrusion = _part.NewEntity((short)KSConstants.o3d_cutExtrusion);
                ksCutExtrusionDefinition cutExtrusionDefinition = entityCutExtrusion.GetDefinition();
                cutExtrusionDefinition.cut = true;
                cutExtrusionDefinition.directionType = dtNormal;
                cutExtrusionDefinition.SetSideParam(true, etBlind, parameters.RadiusBottom * 4, 0, false);
                cutExtrusionDefinition.SetSketch(sketchHoles);
                entityCutExtrusion.Create();
                #endregion

                #region Смещеная плоскость для упоров ножек стойки
                _part = _doc3D.GetPart((short)KSConstants.pTop_part);
                ksEntity entityDisplacedStandStops = _part.GetDefaultEntity((short)KSConstants.o3d_planeXOY);
                ksEntity displacedPlaneStandStops = _part.NewEntity((short)KSConstants.o3d_planeOffset);
                ksPlaneOffsetDefinition planeOffsetDefinitionStandStops = displacedPlaneStandStops.GetDefinition();
                planeOffsetDefinitionStandStops.direction = true;
                planeOffsetDefinitionStandStops.offset = parameters.StandHeight;
                planeOffsetDefinitionStandStops.SetPlane(entityDisplacedStandStops);
                displacedPlaneStandStops.Create();
                #endregion

                #region Смещеная плоcкость для перекладины стойки
                _part = _doc3D.GetPart((short)KSConstants.pTop_part);
                ksEntity entityDisplacedStandBeam = _part.GetDefaultEntity((short)KSConstants.o3d_planeXOY);

                ksEntity displacedPlaneStandBeam = _part.NewEntity((short)KSConstants.o3d_planeOffset);
                ksPlaneOffsetDefinition planeOffsetDefinitionStandBeam = displacedPlaneStandBeam.GetDefinition();
                planeOffsetDefinitionStandBeam.direction = true;
                planeOffsetDefinitionStandBeam.offset = parameters.StandHeight - 5 * 10;
                planeOffsetDefinitionStandBeam.SetPlane(entityDisplacedStandBeam);
                displacedPlaneStandBeam.Create();
                #endregion

                #region смещеная плоскость для ножек стойки
                _part = _doc3D.GetPart((short)KSConstants.pTop_part);
                ksEntity entityDisplacedStandLeg = _part.GetDefaultEntity((short)KSConstants.o3d_planeXOY);

                ksEntity displacedPlaneStandLeg = _part.NewEntity((short)KSConstants.o3d_planeOffset);
                ksPlaneOffsetDefinition planeOffsetDefinitionStandLeg = displacedPlaneStandLeg.GetDefinition();
                planeOffsetDefinitionStandLeg.direction = true;
                planeOffsetDefinitionStandLeg.offset = parameters.StandHeight - 1 * 10;
                planeOffsetDefinitionStandLeg.SetPlane(entityDisplacedStandLeg);
                displacedPlaneStandLeg.Create();
                #endregion

                #region смещеная плоскость для крепления ножек стойки
                _part = _doc3D.GetPart((short)KSConstants.pTop_part);
                ksEntity entityDisplacedStandBracingLeft = _part.GetDefaultEntity((short)KSConstants.o3d_planeXOY);

                ksEntity displacedPlaneStandBracingLeft = _part.NewEntity((short)KSConstants.o3d_planeOffset);
                ksPlaneOffsetDefinition planeOffsetDefinitionStandBracingLeft = displacedPlaneStandBracingLeft.GetDefinition();
                planeOffsetDefinitionStandBracingLeft.direction = true;
                planeOffsetDefinitionStandBracingLeft.offset = 10 * 10;
                planeOffsetDefinitionStandBracingLeft.SetPlane(entityDisplacedStandBracingLeft);
                displacedPlaneStandBracingLeft.Create();
                #endregion

                #region эскиз для крепления ножек стойки
                double offset = 0;
                if (Math.Abs(parameters.RadiusBottom - parameters.RadiusTop) == 0)
                {
                    offset = 20;
                }
                ksEntity entityStandBracing = _part.NewEntity((short)KSConstants.o3d_sketch);
                ksSketchDefinition sketchStandBracing = entityStandBracing.GetDefinition();
                sketchStandBracing.SetPlane(displacedPlaneStandBracingLeft);
                entityStandBracing.Create();
                ksDocument2D document2DStandBracing = sketchStandBracing.BeginEdit();
                _lineParam = _kompas.GetParamStruct((short)KSConstants.ko_LineSegParam);
                _lineParam.x1 = 0;
                _lineParam.x2 = 0.5 * 10;
                _lineParam.y1 = parameters.RadiusBottom + 1 * 10;
                _lineParam.y2 = parameters.RadiusBottom + 1 * 10;
                _lineParam.style = 1;
                document2DStandBracing.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _circleParam.style);
                _lineParam.x1 = 0.5 * 10;
                _lineParam.x2 = 0.5 * 10;
                _lineParam.y1 = parameters.RadiusBottom + 1 * 10;
                _lineParam.y2 = parameters.RadiusBottom - offset - Math.Abs(parameters.RadiusBottom - parameters.RadiusTop);
                _lineParam.style = 1;
                document2DStandBracing.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _circleParam.style);
                _lineParam.x1 = 0.5 * 10;
                _lineParam.x2 = 3.5 * 10;
                _lineParam.y1 = parameters.RadiusBottom - offset - Math.Abs(parameters.RadiusBottom - parameters.RadiusTop);
                _lineParam.y2 = parameters.RadiusBottom - offset - Math.Abs(parameters.RadiusBottom - parameters.RadiusTop);
                _lineParam.style = 1;
                document2DStandBracing.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _circleParam.style);
                _lineParam.x1 = 3.5 * 10;
                _lineParam.x2 = 3.5 * 10;
                _lineParam.y1 = parameters.RadiusBottom - offset - Math.Abs(parameters.RadiusBottom - parameters.RadiusTop);
                _lineParam.y2 = parameters.RadiusBottom - (offset+10) - Math.Abs(parameters.RadiusBottom - parameters.RadiusTop);
                _lineParam.style = 1;
                document2DStandBracing.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _circleParam.style);
                _lineParam.x1 = 3.5 * 10;
                _lineParam.x2 = 0.5 * 10;
                _lineParam.y1 = parameters.RadiusBottom -(offset+10) - Math.Abs(parameters.RadiusBottom - parameters.RadiusTop);
                _lineParam.y2 = parameters.RadiusBottom -(offset+10) - Math.Abs(parameters.RadiusBottom - parameters.RadiusTop);
                _lineParam.style = 1;
                document2DStandBracing.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _circleParam.style);

                //правая
                _lineParam.x1 = 0;
                _lineParam.x2 = 0.5 * 10;
                _lineParam.y1 = -(parameters.RadiusBottom + 1 * 10);
                _lineParam.y2 = -(parameters.RadiusBottom + 1 * 10);
                _lineParam.style = 1;
                document2DStandBracing.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _circleParam.style);
                _lineParam.x1 = 0.5 * 10;
                _lineParam.x2 = 0.5 * 10;
                _lineParam.y1 = -(parameters.RadiusBottom + 1 * 10);
                _lineParam.y2 = -(parameters.RadiusBottom - offset - Math.Abs(parameters.RadiusBottom - parameters.RadiusTop));
                _lineParam.style = 1;
                document2DStandBracing.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _circleParam.style);
                _lineParam.x1 = 0.5 * 10;
                _lineParam.x2 = 3.5 * 10;
                _lineParam.y1 = -(parameters.RadiusBottom - offset - Math.Abs(parameters.RadiusBottom - parameters.RadiusTop));
                _lineParam.y2 = -(parameters.RadiusBottom - offset - Math.Abs(parameters.RadiusBottom - parameters.RadiusTop));
                _lineParam.style = 1;
                document2DStandBracing.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _circleParam.style);
                _lineParam.x1 = 3.5 * 10;
                _lineParam.x2 = 3.5 * 10;
                _lineParam.y1 = -(parameters.RadiusBottom - offset - Math.Abs(parameters.RadiusBottom - parameters.RadiusTop));
                _lineParam.y2 = -(parameters.RadiusBottom - (offset+10) - Math.Abs(parameters.RadiusBottom - parameters.RadiusTop));
                _lineParam.style = 1;
                document2DStandBracing.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _circleParam.style);
                _lineParam.x1 = 3.5 * 10;
                _lineParam.x2 = 0.5 * 10;
                _lineParam.y1 = -(parameters.RadiusBottom - (offset + 10) - Math.Abs(parameters.RadiusBottom - parameters.RadiusTop));
                _lineParam.y2 = -(parameters.RadiusBottom - (offset + 10) - Math.Abs(parameters.RadiusBottom - parameters.RadiusTop));
                _lineParam.style = 1;
                document2DStandBracing.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _circleParam.style);
                //ось 
                ksAxisLineParam axisLineParam = _kompas.GetParamStruct((short)KSConstants.ko_AxisLineParam);
                ksMathPointParam begpoint = axisLineParam.GetBegPoint();
                ksMathPointParam endpoint = axisLineParam.GetEndPoint();
                begpoint.x = 0;
                begpoint.y = 0;
                endpoint.x = 0;
                endpoint.y = 4 * 10;
                document2DStandBracing.ksAxisLine(axisLineParam);

                sketchStandBracing.EndEdit();
                #endregion

                #region Выдаыливание вращением упоров ножек стойки
                ksEntity entityRotateExtrusion = _part.NewEntity((short)KSConstants.o3d_baseRotated);
                ksBaseRotatedDefinition rotatedDefinition = entityRotateExtrusion.GetDefinition();
                rotatedDefinition.directionType = dtNormal;
                rotatedDefinition.toroidShapeType = false;
                rotatedDefinition.SetSideParam(true, 360);
                rotatedDefinition.SetSketch(sketchStandBracing);
                entityRotateExtrusion.Create();
                #endregion
                
                #region эскиз для упоров ножек стойки
                ksEntity entitStandStops = _part.NewEntity((short)KSConstants.o3d_sketch);
                ksSketchDefinition sketchStandStops = entitStandStops.GetDefinition();
                sketchStandStops.SetPlane(displacedPlaneStandStops);
                entitStandStops.Create();
                ksDocument2D document2DStandStops = sketchStandStops.BeginEdit();
                _rectangleParam = _kompas.GetParamStruct((short)KSConstants.ko_RectangleParam);
                _rectangleParam.ang = 0;
                _rectangleParam.y = parameters.RadiusBottom + 1 * 10;
                _rectangleParam.x = parameters.RadiusBottom;
                _rectangleParam.width = -2*(parameters.RadiusBottom + 1 * 10);
                _rectangleParam.height = 1 * 10;
                _rectangleParam.style = 1;
                document2DStandStops.ksRectangle(_rectangleParam, 0);
                _rectangleParam.ang = 0;
                _rectangleParam.y = -(parameters.RadiusBottom + 1 * 10);
                _rectangleParam.x = -parameters.RadiusBottom;
                _rectangleParam.width = 2*(parameters.RadiusBottom + 1 * 10);
                _rectangleParam.height = -1 * 10;
                _rectangleParam.style = 1;
                document2DStandStops.ksRectangle(_rectangleParam, 0);

                sketchStandStops.EndEdit();
                #endregion

                #region Выдавливание  упоров
                ksEntity entityBaseExtrusion = _part.NewEntity((short)KSConstants.o3d_bossExtrusion);
                ksBossExtrusionDefinition extrusionDefinition = entityBaseExtrusion.GetDefinition();
                extrusionDefinition.directionType = 1;
                extrusionDefinition.SetSideParam(false, etBlind, 1 * 10, 0, false);
                extrusionDefinition.SetSketch(sketchStandStops);
                entityBaseExtrusion.Create();
                #endregion

                #region эскиз для ножек
                ksEntity entitStandLeg = _part.NewEntity((short)KSConstants.o3d_sketch);
                ksSketchDefinition sketchStandLeg = entitStandLeg.GetDefinition();
                sketchStandLeg.SetPlane(displacedPlaneStandLeg);
                entitStandLeg.Create();
                ksDocument2D document2DStandLeg = sketchStandLeg.BeginEdit();
                _rectangleParam = _kompas.GetParamStruct((short)KSConstants.ko_RectangleParam);
                _rectangleParam.ang = 0;
                _rectangleParam.y = parameters.RadiusBottom + 1 * 10;
                _rectangleParam.x = 0.5 * 10;
                _rectangleParam.width = -1 * 10;
                _rectangleParam.height = 1 * 10;
                _rectangleParam.style = 1;
                document2DStandLeg.ksRectangle(_rectangleParam, 0);
                _rectangleParam.ang = 0;
                _rectangleParam.y = -(parameters.RadiusBottom + 1 * 10);
                _rectangleParam.x = -0.5 * 10;
                _rectangleParam.width = 1 * 10;
                _rectangleParam.height = -1 * 10;
                _rectangleParam.style = 1;
                document2DStandLeg.ksRectangle(_rectangleParam, 0);

                sketchStandLeg.EndEdit();
                #endregion

                #region Выдавливание  ножек
                ksEntity entityBaseExtrusionLeg = _part.NewEntity((short)KSConstants.o3d_bossExtrusion);
                ksBossExtrusionDefinition extrusionDefinitionLeg = entityBaseExtrusionLeg.GetDefinition();
                extrusionDefinitionLeg.directionType = 1;
                extrusionDefinitionLeg.SetSideParam(false, etBlind, parameters.StandHeight -1 * 10 , 0, false);
                extrusionDefinitionLeg.SetSketch(sketchStandLeg);
                entityBaseExtrusionLeg.Create();
                #endregion

                #region эскиз перекладины
                ksEntity entitStandBeam = _part.NewEntity((short)KSConstants.o3d_sketch);
                ksSketchDefinition sketchStandBeam = entitStandBeam.GetDefinition();
                sketchStandBeam.SetPlane(displacedPlaneStandBeam);
                entitStandBeam.Create();
                ksDocument2D document2DStandBeam = sketchStandBeam.BeginEdit();
                _rectangleParam = _kompas.GetParamStruct((short)KSConstants.ko_RectangleParam);
                _rectangleParam.ang = 0;
                _rectangleParam.y = parameters.RadiusBottom+1*10;
                _rectangleParam.x = -0.5*10;
                _rectangleParam.height = -2*(parameters.RadiusBottom+1 * 10);
                _rectangleParam.width = 1 * 10;
                _rectangleParam.style = 1;
                document2DStandBeam.ksRectangle(_rectangleParam, 0);

                sketchStandBeam.EndEdit();
                #endregion

                #region Выдавливание  перекладины
                ksEntity entityBaseExtrusionBeam = _part.NewEntity((short)KSConstants.o3d_bossExtrusion);
                ksBossExtrusionDefinition extrusionDefinitionBeam = entityBaseExtrusionBeam.GetDefinition();
                extrusionDefinitionBeam.SetSideParam(true, etBlind, 1*10, 0, false);
                extrusionDefinitionBeam.SetSketch(sketchStandBeam);
                entityBaseExtrusionBeam.Create();
                #endregion

            }

            if (parameters.Ashtray)
            {
                AshtrayBuild(parameters);
            }
        }
    }
}
