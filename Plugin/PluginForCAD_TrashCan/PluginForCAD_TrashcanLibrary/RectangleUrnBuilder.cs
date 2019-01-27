using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kompas6API5;

namespace PluginForCAD_TrashcanLibrary
{
    public class RectangleUrnBuilder
    {
        private KompasObject _kompas;

        private ksEntity _entity;
        private ksPart _part;
        private ksCircleParam _circleParam;
        private ksRectangleParam _rectangleParam;
        private ksLineSegParam _lineParam;
        private Document3D _doc3D;


        private void AshtrayBuild(RectangleParameters parameters)
        {
            const double ashtrayHihght = 30;
            const double ashtrayThickness = 5;
            const double ashtrayRadius = 30;
            #region Смещение плоскости для основания пепельницы
            _part = _doc3D.GetPart((short)KSConstants.pTop_part);
            ksEntity EntityAshtrayPlane = _part.GetDefaultEntity((short)KSConstants.o3d_planeXOY);

            ksEntity ashtrayPlane = _part.NewEntity((short)KSConstants.o3d_planeOffset);
            ksPlaneOffsetDefinition ashtrayPlaneOffsetDefinition = ashtrayPlane.GetDefinition();
            ashtrayPlaneOffsetDefinition.direction = true;
            ashtrayPlaneOffsetDefinition.offset = ashtrayHihght;
            ashtrayPlaneOffsetDefinition.SetPlane(EntityAshtrayPlane);
            ashtrayPlane.Create();
            #endregion

            #region Смещение плоскости для основания толщины пепельницы
            _part = _doc3D.GetPart((short)KSConstants.pTop_part);
            ksEntity EntityAshtrayThicknessPlane = _part.GetDefaultEntity((short)KSConstants.o3d_planeXOY);

            ksEntity ashtrayThicknessPlane = _part.NewEntity((short)KSConstants.o3d_planeOffset);
            ksPlaneOffsetDefinition ashtrayThicknessPlaneOffsetDefinition = ashtrayThicknessPlane.GetDefinition();
            ashtrayThicknessPlaneOffsetDefinition.direction = true;
            ashtrayThicknessPlaneOffsetDefinition.offset = ashtrayHihght - ashtrayThickness;
            ashtrayThicknessPlaneOffsetDefinition.SetPlane(EntityAshtrayThicknessPlane);
            ashtrayThicknessPlane.Create();
            #endregion

            #region Эскиз основания пепельницы

            _part = _doc3D.GetPart((short)KSConstants.pTop_part);
            ksEntity ashtrayEntity = _part.NewEntity((short)KSConstants.o3d_sketch);
            ksSketchDefinition ashtraySketchDefinition = ashtrayEntity.GetDefinition();
            ashtraySketchDefinition.SetPlane(ashtrayPlane);
            ashtrayEntity.Create();
            ksDocument2D Document2DAshtray = ashtraySketchDefinition.BeginEdit();
            _lineParam = _kompas.GetParamStruct((short)KSConstants.ko_LineSegParam);
            _lineParam.y1 = -ashtrayRadius;
            _lineParam.x1 = parameters.WidthBottom / 2 - parameters.WallThickness / 2;
            _lineParam.x2 = _lineParam.x1;
            _lineParam.y2 = ashtrayRadius;
            _lineParam.style = 1;
            Document2DAshtray.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _lineParam.style);

            var arcLeftPointX = _lineParam.x1;
            var arcLeftPointY = _lineParam.y1;
            var arcRightPointX = _lineParam.x2;
            var arcRightPointY = _lineParam.y2;
            var arcMidlePointX = _lineParam.x1 - ashtrayRadius;
            var arcMidlePointY = 0;

            Document2DAshtray.ksArcBy3Points(arcLeftPointX,arcLeftPointY,arcMidlePointX,arcMidlePointY,arcRightPointX,arcRightPointY,1);
            ashtraySketchDefinition.EndEdit();
            #endregion

            #region Эскиз верха основания пепельницы

            _part = _doc3D.GetPart((short)KSConstants.pTop_part);
            ksEntity ashtrayTopEntity = _part.NewEntity((short)KSConstants.o3d_sketch);
            ksSketchDefinition ashtrayTopSketchDefinition = ashtrayTopEntity.GetDefinition();
            ashtrayTopSketchDefinition.SetPlane(ashtrayThicknessPlane);
            ashtrayTopEntity.Create();
            ksDocument2D Document2DAshtrayTop = ashtrayTopSketchDefinition.BeginEdit();
            _lineParam = _kompas.GetParamStruct((short)KSConstants.ko_LineSegParam);
            _lineParam.y1 = -ashtrayRadius+ ashtrayThickness;
            _lineParam.x1 = parameters.WidthBottom / 2 - parameters.WallThickness / 2;
            _lineParam.x2 = _lineParam.x1;
            _lineParam.y2 = ashtrayRadius - ashtrayThickness;
            _lineParam.style = 1;
            Document2DAshtrayTop.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _lineParam.style);

            arcLeftPointX = _lineParam.x1;
            arcLeftPointY = _lineParam.y1;
            arcRightPointX = _lineParam.x2;
            arcRightPointY = _lineParam.y2;
            arcMidlePointX = _lineParam.x1 - ashtrayRadius + ashtrayThickness;
            arcMidlePointY = 0-5;

            Document2DAshtrayTop.ksArcBy3Points(arcLeftPointX, arcLeftPointY, arcMidlePointX, arcMidlePointY, arcRightPointX, arcRightPointY, 1);
            ashtrayTopSketchDefinition.EndEdit();
            #endregion

            #region Выдавливание пепельницы
            const short etBlind = 0;
            const short dtNormal = 0;
            const short dtReverse = 1;
            ksEntity EntityBaseExtrusionAshtray = _part.NewEntity((short)KSConstants.o3d_bossExtrusion);
            ksBossExtrusionDefinition extrusionDefinitionAshtray = EntityBaseExtrusionAshtray.GetDefinition();
            extrusionDefinitionAshtray.directionType = dtReverse;
            extrusionDefinitionAshtray.SetSideParam(false, etBlind, ashtrayHihght, 0, false);
            extrusionDefinitionAshtray.SetSketch(ashtraySketchDefinition);
            EntityBaseExtrusionAshtray.Create();
            #endregion

            #region Вырезание полости пепельницы
            ksEntity EntityCutExtrusionAshtray = _part.NewEntity((short)KSConstants.o3d_cutExtrusion);
            ksCutExtrusionDefinition cutExtrusionDefinitionAshtray = EntityCutExtrusionAshtray.GetDefinition();
            cutExtrusionDefinitionAshtray.cut = true;
            cutExtrusionDefinitionAshtray.directionType = dtNormal;
            cutExtrusionDefinitionAshtray.SetSideParam(true, etBlind, ashtrayHihght-ashtrayThickness, 0, false);
            cutExtrusionDefinitionAshtray.SetSketch(ashtrayTopSketchDefinition);
            EntityCutExtrusionAshtray.Create();
            #endregion

            #region Эскиз отверстий основания пепельницы

            _part = _doc3D.GetPart((short)KSConstants.pTop_part);
            ksEntity ashtrayHoleEntity = _part.NewEntity((short)KSConstants.o3d_sketch);
            ksSketchDefinition ashtrayHoleSketchDefinition = ashtrayHoleEntity.GetDefinition();
            ashtrayHoleSketchDefinition.SetPlane(ashtrayThicknessPlane);
            ashtrayHoleEntity.Create();
            ksDocument2D Document2DAshtrayHole = ashtrayHoleSketchDefinition.BeginEdit();
            _circleParam = _kompas.GetParamStruct((short)KSConstants.ko_CircleParam);
            _circleParam.xc = ((parameters.WidthBottom / 2 - parameters.WallThickness / 2)+(parameters.WidthBottom / 2 - parameters.WallThickness / 2 - ashtrayRadius + ashtrayThickness))/2;
            _circleParam.yc = 0;
            _circleParam.rad = 0.5;
            _circleParam.style = 1;
            Document2DAshtrayHole.ksCircle(_circleParam.xc, _circleParam.yc, _circleParam.rad, _circleParam.style);

            _circleParam.yc = 5;
            _circleParam.style = 1;
            Document2DAshtrayHole.ksCircle(_circleParam.xc, _circleParam.yc, _circleParam.rad, _circleParam.style);

            _circleParam.yc = -5;
            _circleParam.style = 1;
            Document2DAshtrayHole.ksCircle(_circleParam.xc, _circleParam.yc, _circleParam.rad, _circleParam.style);

            ashtrayHoleSketchDefinition.EndEdit();
            #endregion

            #region Вырезание отверстий
            ksEntity EntityCutExtrusionAshtrayHole = _part.NewEntity((short)KSConstants.o3d_cutExtrusion);
            ksCutExtrusionDefinition cutExtrusionDefinitionAshtrayHole = EntityCutExtrusionAshtrayHole.GetDefinition();
            cutExtrusionDefinitionAshtrayHole.cut = true;
            cutExtrusionDefinitionAshtrayHole.directionType = dtReverse;
            cutExtrusionDefinitionAshtrayHole.SetSideParam(true, etBlind, ashtrayThickness, 0, false);
            cutExtrusionDefinitionAshtrayHole.SetSketch(ashtrayHoleSketchDefinition);
            EntityCutExtrusionAshtrayHole.Create();
            #endregion

        }

        public RectangleUrnBuilder(KompasObject kompas)
        {
            _kompas = kompas;
        }

        public void Build(RectangleParameters parameters)
        {
            _doc3D = _kompas.Document3D();
            _doc3D.Create(false, true);

            #region Эскиз нижненго основания

            _part = _doc3D.GetPart((short)KSConstants.pTop_part);
            _entity = _part.NewEntity((short)KSConstants.o3d_sketch);
            ksSketchDefinition sketchDefinition = _entity.GetDefinition();
            ksEntity XOYPlane = _part.GetDefaultEntity((short)KSConstants.o3d_planeXOY);
            sketchDefinition.SetPlane(XOYPlane);
            _entity.Create();
            ksDocument2D Document2D = sketchDefinition.BeginEdit();

            _rectangleParam = _kompas.GetParamStruct((short)KSConstants.ko_RectangleParam);
            _rectangleParam.x = parameters.LengthBottom/2;
            _rectangleParam.y = -parameters.WidthBottom/2;
            _rectangleParam.height = parameters.LengthBottom;
            _rectangleParam.width = -parameters.WidthBottom;
            _rectangleParam.style = 1;
            Document2D.ksRectangle(_rectangleParam, 0);
            sketchDefinition.EndEdit();
            #endregion

            #region Смещение плоскости для верхнего основания
            _part = _doc3D.GetPart((short)KSConstants.pTop_part);
            ksEntity EntityDisplacedPlane = _part.GetDefaultEntity((short)KSConstants.o3d_planeXOY);

            ksEntity displacedPlane = _part.NewEntity((short)KSConstants.o3d_planeOffset);
            ksPlaneOffsetDefinition planeOffsetDefinition = displacedPlane.GetDefinition();
            planeOffsetDefinition.direction = true;
            planeOffsetDefinition.offset = parameters.UrnHeight;
            planeOffsetDefinition.SetPlane(EntityDisplacedPlane);
            displacedPlane.Create();
            #endregion

            #region Эскиз верхнегого основания
            ksEntity EntityTop = _part.NewEntity((short)KSConstants.o3d_sketch);
            ksSketchDefinition sketchDefinitionTop = EntityTop.GetDefinition();
            sketchDefinitionTop.SetPlane(displacedPlane);
            EntityTop.Create();
            ksDocument2D Document2DTop = sketchDefinitionTop.BeginEdit();
            _rectangleParam = _kompas.GetParamStruct((short)KSConstants.ko_RectangleParam);
            _rectangleParam.x = parameters.LengthTop / 2;
            _rectangleParam.y = -parameters.WidthTop / 2;
            _rectangleParam.height = parameters.LengthTop;
            _rectangleParam.width = -parameters.WidthTop;
            _rectangleParam.style = 1;
            Document2DTop.ksRectangle(_rectangleParam, 0);
            sketchDefinitionTop.EndEdit();
            #endregion

            #region Операция выдавливания по сечениям
            ksEntity EntityLoft = _part.NewEntity((short)KSConstants.o3d_baseLoft);
            ksBaseLoftDefinition loftDefinition = EntityLoft.GetDefinition();
            loftDefinition.SetLoftParam(false, true, true);
            ksEntityCollection entityCollection = loftDefinition.Sketchs();
            entityCollection.Clear();
            entityCollection.Add(sketchDefinition);
            entityCollection.Add(sketchDefinitionTop);
            EntityLoft.Create();
            #endregion

            #region Смещение плоскости для вырезания верхнего основания
            _part = _doc3D.GetPart((short)KSConstants.pTop_part);
            ksEntity EntityDisplacedPlaneTopCut = _part.GetDefaultEntity((short)KSConstants.o3d_planeXOY);

            ksEntity displacedPlaneTopCut = _part.NewEntity((short)KSConstants.o3d_planeOffset);
            ksPlaneOffsetDefinition planeOffsetDefinitionTop = displacedPlaneTopCut.GetDefinition();
            planeOffsetDefinitionTop.direction = true;
            planeOffsetDefinitionTop.offset = 0;
            planeOffsetDefinitionTop.SetPlane(EntityDisplacedPlaneTopCut);
            displacedPlaneTopCut.Create();
            #endregion

            #region Смещение плоскости для вырезания нижнего основания
            _part = _doc3D.GetPart((short)KSConstants.pTop_part);
            ksEntity EntityDisplacedPlaneBotCut = _part.GetDefaultEntity((short)KSConstants.o3d_planeXOY);

            ksEntity displacedPlaneBotCut = _part.NewEntity((short)KSConstants.o3d_planeOffset);
            ksPlaneOffsetDefinition planeOffsetDefinitionBot = displacedPlaneBotCut.GetDefinition();
            planeOffsetDefinitionBot.direction = true;
            planeOffsetDefinitionBot.offset = parameters.UrnHeight - parameters.BottomThickness;
            planeOffsetDefinitionBot.SetPlane(EntityDisplacedPlaneBotCut);
            displacedPlaneBotCut.Create();
            #endregion

            #region Эскиз верхнего основания для вырезания
            ksEntity EntityTopCut = _part.NewEntity((short)KSConstants.o3d_sketch);
            ksSketchDefinition sketchTopCut = EntityTopCut.GetDefinition();
            sketchTopCut.SetPlane(displacedPlaneBotCut);
            EntityTopCut.Create();
            ksDocument2D Document2DTopCut = sketchTopCut.BeginEdit();
            _rectangleParam = _kompas.GetParamStruct((short)KSConstants.ko_RectangleParam);
            _rectangleParam.x = parameters.LengthTop / 2 -parameters.WallThickness;
            _rectangleParam.y = -(parameters.WidthTop / 2 - parameters.WallThickness);
            _rectangleParam.height = parameters.LengthTop - parameters.WallThickness*2;
            _rectangleParam.width = -(parameters.WidthTop - parameters.WallThickness*2);
            _rectangleParam.style = 1;
            Document2DTopCut.ksRectangle(_rectangleParam, 0);
            sketchTopCut.EndEdit();
            #endregion

            #region Эскиз нижнего основания для вырезания
            ksEntity EntityBotCut = _part.NewEntity((short)KSConstants.o3d_sketch);
            ksSketchDefinition sketchBotCut = EntityBotCut.GetDefinition();
            sketchBotCut.SetPlane(displacedPlaneTopCut);
            EntityBotCut.Create();
            ksDocument2D Document2DBotCut = sketchBotCut.BeginEdit();
            _rectangleParam = _kompas.GetParamStruct((short)KSConstants.ko_RectangleParam);
            _rectangleParam.x = parameters.LengthBottom / 2 - parameters.WallThickness ;
            _rectangleParam.y = -(parameters.WidthBottom / 2 - parameters.WallThickness );
            _rectangleParam.height = parameters.LengthBottom - parameters.WallThickness*2;
            _rectangleParam.width = -(parameters.WidthBottom - parameters.WallThickness*2);
            _rectangleParam.style = 1;
            Document2DBotCut.ksRectangle(_rectangleParam, 0);
            sketchBotCut.EndEdit();
            #endregion

            #region Вырезание по сечениям
            ksEntity EntityCutLoft = _part.NewEntity((short)KSConstants.o3d_cutLoft);
            ksCutLoftDefinition cutLoftDefinition = EntityCutLoft.GetDefinition();
            cutLoftDefinition.SetLoftParam(false, true, true);
            cutLoftDefinition.cut = true;
            ksEntityCollection entityCollectionCut = cutLoftDefinition.Sketchs();

            entityCollectionCut.Clear();
            entityCollectionCut.Add(sketchBotCut);
            entityCollectionCut.Add(sketchTopCut);
            EntityCutLoft.Create();
            #endregion
            //Если есть стойка
            if (parameters.Stand)
            {
                #region Смещеная плоскость для отверстий
                _part = _doc3D.GetPart((short)KSConstants.pTop_part);
                ksEntity EntityDisplacedPlaneHoles = _part.GetDefaultEntity((short)KSConstants.o3d_planeXOZ);

                ksEntity displacedPlaneHoles = _part.NewEntity((short)KSConstants.o3d_planeOffset);
                ksPlaneOffsetDefinition planeOffsetDefinitionHoles = displacedPlaneHoles.GetDefinition();
                planeOffsetDefinitionHoles.direction = true;
                planeOffsetDefinitionHoles.offset = parameters.WidthBottom/2 + 2 * 10;
                planeOffsetDefinitionHoles.SetPlane(EntityDisplacedPlaneHoles);
                displacedPlaneHoles.Create();
                #endregion

                #region Эскиз для отверстия
                ksEntity EntityHoles = _part.NewEntity((short)KSConstants.o3d_sketch);
                //ksEntity XOYPlaneBotCut = _part.GetDefaultEntity((short)KSConstants.o3d_planeXOY);
                ksSketchDefinition sketchHoles = EntityHoles.GetDefinition();
                sketchHoles.SetPlane(displacedPlaneHoles);
                EntityHoles.Create();
                ksDocument2D Document2DHoles = sketchHoles.BeginEdit();
                _circleParam = _kompas.GetParamStruct((short)KSConstants.ko_CircleParam);
                _circleParam.xc = 0;
                _circleParam.yc = -10 * 10;
                _circleParam.rad = 2.5 * 10;
                _circleParam.style = 1;
                Document2DHoles.ksCircle(_circleParam.xc, _circleParam.yc, _circleParam.rad, _circleParam.style);
                sketchHoles.EndEdit();
                #endregion

                #region Отверстия
                const short etBlind = 0;
                const short dtNormal = 0;
                ksEntity EntityCutExtrusion = _part.NewEntity((short)KSConstants.o3d_cutExtrusion);
                ksCutExtrusionDefinition cutExtrusionDefinition = EntityCutExtrusion.GetDefinition();
                cutExtrusionDefinition.cut = true;
                cutExtrusionDefinition.directionType = dtNormal;
                cutExtrusionDefinition.SetSideParam(true, etBlind, parameters.WidthBottom / 2 * 4, 0, false);
                cutExtrusionDefinition.SetSketch(sketchHoles);
                EntityCutExtrusion.Create();
                #endregion

                #region Смещеная плоскость для упоров ножек стойки
                _part = _doc3D.GetPart((short)KSConstants.pTop_part);
                ksEntity EntityDisplacedStandStops = _part.GetDefaultEntity((short)KSConstants.o3d_planeXOY);

                ksEntity displacedPlaneStandStops = _part.NewEntity((short)KSConstants.o3d_planeOffset);
                ksPlaneOffsetDefinition planeOffsetDefinitionStandStops = displacedPlaneStandStops.GetDefinition();
                planeOffsetDefinitionStandStops.direction = true;
                planeOffsetDefinitionStandStops.offset = parameters.StandHeight;
                planeOffsetDefinitionStandStops.SetPlane(EntityDisplacedStandStops);
                displacedPlaneStandStops.Create();
                #endregion

                #region Смещеная плоcкость для перекладины стойки
                _part = _doc3D.GetPart((short)KSConstants.pTop_part);
                ksEntity EntityDisplacedStandBeam = _part.GetDefaultEntity((short)KSConstants.o3d_planeXOY);

                ksEntity displacedPlaneStandBeam = _part.NewEntity((short)KSConstants.o3d_planeOffset);
                ksPlaneOffsetDefinition planeOffsetDefinitionStandBeam = displacedPlaneStandBeam.GetDefinition();
                planeOffsetDefinitionStandBeam.direction = true;
                planeOffsetDefinitionStandBeam.offset = parameters.StandHeight - 5 * 10;
                planeOffsetDefinitionStandBeam.SetPlane(EntityDisplacedStandBeam);
                displacedPlaneStandBeam.Create();
                #endregion

                #region смещеная плоскость для ножек стойки
                _part = _doc3D.GetPart((short)KSConstants.pTop_part);
                ksEntity EntityDisplacedStandLeg = _part.GetDefaultEntity((short)KSConstants.o3d_planeXOY);

                ksEntity displacedPlaneStandLeg = _part.NewEntity((short)KSConstants.o3d_planeOffset);
                ksPlaneOffsetDefinition planeOffsetDefinitionStandLeg = displacedPlaneStandLeg.GetDefinition();
                planeOffsetDefinitionStandLeg.direction = true;
                planeOffsetDefinitionStandLeg.offset = parameters.StandHeight - 1 * 10;
                planeOffsetDefinitionStandLeg.SetPlane(EntityDisplacedStandLeg);
                displacedPlaneStandLeg.Create();
                #endregion

                #region смещеная плоскость для крепления ножек стойки
                _part = _doc3D.GetPart((short)KSConstants.pTop_part);
                ksEntity EntityDisplacedStandBracingLeft = _part.GetDefaultEntity((short)KSConstants.o3d_planeXOY);

                ksEntity displacedPlaneStandBracingLeft = _part.NewEntity((short)KSConstants.o3d_planeOffset);
                ksPlaneOffsetDefinition planeOffsetDefinitionStandBracingLeft = displacedPlaneStandBracingLeft.GetDefinition();
                planeOffsetDefinitionStandBracingLeft.direction = true;
                planeOffsetDefinitionStandBracingLeft.offset = 10 * 10;
                planeOffsetDefinitionStandBracingLeft.SetPlane(EntityDisplacedStandBracingLeft);
                displacedPlaneStandBracingLeft.Create();
                #endregion

                #region эскиз для крепления ножек стойки
                double offset = 0;
                if (Math.Abs(parameters.WidthBottom / 2 - parameters.WidthTop / 2) == 0)
                {
                    offset = 15;
                }
                ksEntity EntityStandBracing = _part.NewEntity((short)KSConstants.o3d_sketch);
                ksSketchDefinition sketchStandBracing = EntityStandBracing.GetDefinition();
                sketchStandBracing.SetPlane(displacedPlaneStandBracingLeft);
                EntityStandBracing.Create();
                ksDocument2D Document2DStandBracing = sketchStandBracing.BeginEdit();
                _lineParam = _kompas.GetParamStruct((short)KSConstants.ko_LineSegParam);
                _lineParam.x1 = 0;
                _lineParam.x2 = 0.5 * 10;
                _lineParam.y1 = parameters.WidthBottom / 2 + 1 * 10;
                _lineParam.y2 = parameters.WidthBottom / 2 + 1 * 10;
                _lineParam.style = 1;
                Document2DStandBracing.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _circleParam.style);
                _lineParam.x1 = 0.5 * 10;
                _lineParam.x2 = 0.5 * 10;
                _lineParam.y1 = parameters.WidthBottom / 2 + 1 * 10;
                _lineParam.y2 = parameters.WidthBottom / 2 - offset - Math.Abs(parameters.WidthBottom / 2 - parameters.WidthTop / 2);
                _lineParam.style = 1;
                Document2DStandBracing.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _circleParam.style);
                _lineParam.x1 = 0.5 * 10;
                _lineParam.x2 = 3.5 * 10;
                _lineParam.y1 = parameters.WidthBottom / 2 - offset - Math.Abs(parameters.WidthBottom / 2 - parameters.WidthTop / 2);
                _lineParam.y2 = parameters.WidthBottom / 2 - offset - Math.Abs(parameters.WidthBottom / 2 - parameters.WidthTop / 2);
                _lineParam.style = 1;
                Document2DStandBracing.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _circleParam.style);
                _lineParam.x1 = 3.5 * 10;
                _lineParam.x2 = 3.5 * 10;
                _lineParam.y1 = parameters.WidthBottom / 2 - offset - Math.Abs(parameters.WidthBottom / 2 - parameters.WidthTop / 2);
                _lineParam.y2 = parameters.WidthBottom / 2 - (offset+10) - Math.Abs(parameters.WidthBottom / 2 - parameters.WidthTop / 2);
                _lineParam.style = 1;
                Document2DStandBracing.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _circleParam.style);
                _lineParam.x1 = 3.5 * 10;
                _lineParam.x2 = 0.5 * 10;
                _lineParam.y1 = parameters.WidthBottom / 2 - (offset + 10) - Math.Abs(parameters.WidthBottom / 2 - parameters.WidthTop / 2);
                _lineParam.y2 = parameters.WidthBottom / 2 - (offset + 10) - Math.Abs(parameters.WidthBottom / 2 - parameters.WidthTop / 2);
                _lineParam.style = 1;
                Document2DStandBracing.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _circleParam.style);

                //правая
                _lineParam.x1 = 0;
                _lineParam.x2 = 0.5 * 10;
                _lineParam.y1 = -(parameters.WidthBottom / 2 + 1 * 10);
                _lineParam.y2 = -(parameters.WidthBottom / 2 + 1 * 10);
                _lineParam.style = 1;
                Document2DStandBracing.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _circleParam.style);
                _lineParam.x1 = 0.5 * 10;
                _lineParam.x2 = 0.5 * 10;
                _lineParam.y1 = -(parameters.WidthBottom / 2 + 1 * 10);
                _lineParam.y2 = -(parameters.WidthBottom / 2 - offset - Math.Abs(parameters.WidthBottom / 2 - parameters.WidthTop / 2));
                _lineParam.style = 1;
                Document2DStandBracing.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _circleParam.style);
                _lineParam.x1 = 0.5 * 10;
                _lineParam.x2 = 3.5 * 10;
                _lineParam.y1 = -(parameters.WidthBottom / 2 - offset - Math.Abs(parameters.WidthBottom / 2 - parameters.WidthTop / 2));
                _lineParam.y2 = -(parameters.WidthBottom / 2 - offset - Math.Abs(parameters.WidthBottom / 2 - parameters.WidthTop / 2));
                _lineParam.style = 1;
                Document2DStandBracing.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _circleParam.style);
                _lineParam.x1 = 3.5 * 10;
                _lineParam.x2 = 3.5 * 10;
                _lineParam.y1 = -(parameters.WidthBottom / 2 - offset - Math.Abs(parameters.WidthBottom / 2 - parameters.WidthTop / 2));
                _lineParam.y2 = -(parameters.WidthBottom / 2 - (offset + 10) - Math.Abs(parameters.WidthBottom / 2 - parameters.WidthTop / 2));
                _lineParam.style = 1;
                Document2DStandBracing.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _circleParam.style);
                _lineParam.x1 = 3.5 * 10;
                _lineParam.x2 = 0.5 * 10;
                _lineParam.y1 = -(parameters.WidthBottom / 2 - (offset + 10) - Math.Abs(parameters.WidthBottom / 2 - parameters.WidthTop / 2));
                _lineParam.y2 = -(parameters.WidthBottom / 2 - (offset + 10) - Math.Abs(parameters.WidthBottom / 2 - parameters.WidthTop / 2));
                _lineParam.style = 1;
                Document2DStandBracing.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _circleParam.style);
                //ось 
                ksAxisLineParam axisLineParam = _kompas.GetParamStruct((short)KSConstants.ko_AxisLineParam);
                ksMathPointParam begpoint = axisLineParam.GetBegPoint();
                ksMathPointParam endpoint = axisLineParam.GetEndPoint();
                begpoint.x = 0;
                begpoint.y = 0;
                endpoint.x = 0;
                endpoint.y = 4 * 10;
                Document2DStandBracing.ksAxisLine(axisLineParam);

                sketchStandBracing.EndEdit();
                #endregion

                #region Выдаыливание вращением упоров ножек стойки
                ksEntity EntityRotateExtrusion = _part.NewEntity((short)KSConstants.o3d_baseRotated);
                ksBaseRotatedDefinition rotatedDefinition = EntityRotateExtrusion.GetDefinition();
                rotatedDefinition.directionType = dtNormal;
                rotatedDefinition.toroidShapeType = false;
                rotatedDefinition.SetSideParam(true, 360);
                rotatedDefinition.SetSketch(sketchStandBracing);
                EntityRotateExtrusion.Create();
                #endregion

                #region эскиз для упоров ножек стойки
                ksEntity EntitStandStops = _part.NewEntity((short)KSConstants.o3d_sketch);
                ksSketchDefinition sketchStandStops = EntitStandStops.GetDefinition();
                sketchStandStops.SetPlane(displacedPlaneStandStops);
                EntitStandStops.Create();
                ksDocument2D Document2DStandStops = sketchStandStops.BeginEdit();
                _rectangleParam = _kompas.GetParamStruct((short)KSConstants.ko_RectangleParam);
                _rectangleParam.ang = 0;
                _rectangleParam.y = parameters.WidthBottom / 2 + 1 * 10;
                _rectangleParam.x = parameters.WidthBottom / 2;
                _rectangleParam.width = -2 * (parameters.WidthBottom / 2 );
                _rectangleParam.height = 1 * 10;
                _rectangleParam.style = 1;
                Document2DStandStops.ksRectangle(_rectangleParam, 0);
                _rectangleParam.ang = 0;
                _rectangleParam.y = -(parameters.WidthBottom / 2 + 1 * 10);
                _rectangleParam.x = -parameters.WidthBottom / 2;
                _rectangleParam.width = 2 * (parameters.WidthBottom / 2);
                _rectangleParam.height = -1 * 10;
                _rectangleParam.style = 1;
                Document2DStandStops.ksRectangle(_rectangleParam, 0);

                sketchStandStops.EndEdit();
                #endregion

                #region Выдавливание  упоров
                ksEntity EntityBaseExtrusion = _part.NewEntity((short)KSConstants.o3d_bossExtrusion);
                ksBossExtrusionDefinition extrusionDefinition = EntityBaseExtrusion.GetDefinition();
                extrusionDefinition.directionType = 1;
                extrusionDefinition.SetSideParam(false, etBlind, 1 * 10, 0, false);
                extrusionDefinition.SetSketch(sketchStandStops);
                EntityBaseExtrusion.Create();
                #endregion

                #region эскиз для ножек
                ksEntity EntitStandLeg = _part.NewEntity((short)KSConstants.o3d_sketch);
                ksSketchDefinition sketchStandLeg = EntitStandLeg.GetDefinition();
                sketchStandLeg.SetPlane(displacedPlaneStandLeg);
                EntitStandLeg.Create();
                ksDocument2D Document2DStandLeg = sketchStandLeg.BeginEdit();
                _rectangleParam = _kompas.GetParamStruct((short)KSConstants.ko_RectangleParam);
                _rectangleParam.ang = 0;
                _rectangleParam.y = parameters.WidthBottom / 2 + 1 * 10;
                _rectangleParam.x = 0.5 * 10;
                _rectangleParam.width = -1 * 10;
                _rectangleParam.height = 1 * 10;
                _rectangleParam.style = 1;
                Document2DStandLeg.ksRectangle(_rectangleParam, 0);
                _rectangleParam.ang = 0;
                _rectangleParam.y = -(parameters.WidthBottom / 2 + 1 * 10);
                _rectangleParam.x = -0.5 * 10;
                _rectangleParam.width = 1 * 10;
                _rectangleParam.height = -1 * 10;
                _rectangleParam.style = 1;
                Document2DStandLeg.ksRectangle(_rectangleParam, 0);

                sketchStandLeg.EndEdit();
                #endregion

                #region Выдавливание  ножек
                ksEntity EntityBaseExtrusionLeg = _part.NewEntity((short)KSConstants.o3d_bossExtrusion);
                ksBossExtrusionDefinition extrusionDefinitionLeg = EntityBaseExtrusionLeg.GetDefinition();
                extrusionDefinitionLeg.directionType = 1;
                extrusionDefinitionLeg.SetSideParam(false, etBlind, parameters.StandHeight - 1 * 10, 0, false);
                extrusionDefinitionLeg.SetSketch(sketchStandLeg);
                EntityBaseExtrusionLeg.Create();
                #endregion

                #region эскиз перекладины
                ksEntity EntitStandBeam = _part.NewEntity((short)KSConstants.o3d_sketch);
                ksSketchDefinition sketchStandBeam = EntitStandBeam.GetDefinition();
                sketchStandBeam.SetPlane(displacedPlaneStandBeam);
                EntitStandBeam.Create();
                ksDocument2D Document2DStandBeam = sketchStandBeam.BeginEdit();
                _rectangleParam = _kompas.GetParamStruct((short)KSConstants.ko_RectangleParam);
                _rectangleParam.ang = 0;
                _rectangleParam.y = parameters.WidthBottom / 2 + 1 * 10;
                _rectangleParam.x = -0.5 * 10;
                _rectangleParam.height = -2 * (parameters.WidthBottom / 2 + 1 * 10);
                _rectangleParam.width = 1 * 10;
                _rectangleParam.style = 1;
                Document2DStandBeam.ksRectangle(_rectangleParam, 0);

                sketchStandBeam.EndEdit();
                #endregion

                #region Выдавливание  перекладины
                ksEntity EntityBaseExtrusionBeam = _part.NewEntity((short)KSConstants.o3d_bossExtrusion);
                ksBossExtrusionDefinition extrusionDefinitionBeam = EntityBaseExtrusionBeam.GetDefinition();
                extrusionDefinitionBeam.SetSideParam(true, etBlind, 1 * 10, 0, false);
                extrusionDefinitionBeam.SetSketch(sketchStandBeam);
                EntityBaseExtrusionBeam.Create();
                #endregion
            }
            if (parameters.Ashtray)
            {
                AshtrayBuild(parameters);
            }

        }
    }
}

