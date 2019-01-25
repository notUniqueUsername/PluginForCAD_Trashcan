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
        private KompasObject _kompas;

        private ksEntity _entity;
        private ksPart _part;
        private ksCircleParam _circleParam;
        private ksRectangleParam _rectangleParam;
        private ksLineSegParam _lineParam;
        private Document3D _doc3D;
        private ksCornerParam _corner;

        public CircleUrnBuilder(KompasObject kompas)
        {
            _kompas = kompas;
        }

        public void Build(Parameters parameters)
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

            _circleParam = _kompas.GetParamStruct((short)KSConstants.ko_CircleParam);
            _circleParam.xc = 0;
            _circleParam.yc = 0;
            _circleParam.rad = parameters.RadiusBottom;
            _circleParam.style = 1;
            Document2D.ksCircle(_circleParam.xc, _circleParam.yc, _circleParam.rad, _circleParam.style);
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
            _circleParam = _kompas.GetParamStruct((short)KSConstants.ko_CircleParam);
            _circleParam.xc = 0;
            _circleParam.yc = 0;
            _circleParam.rad = parameters.RadiusTop;
            _circleParam.style = 1;
            Document2DTop.ksCircle(_circleParam.xc, _circleParam.yc, _circleParam.rad, _circleParam.style);
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
            //ksEntity XOYPlaneTopCut = _part.GetDefaultEntity((short)KSConstants.o3d_planeXOY);
            ksSketchDefinition sketchTopCut = EntityTopCut.GetDefinition();
            sketchTopCut.SetPlane(displacedPlaneTopCut);
            EntityTopCut.Create();
            ksDocument2D Document2DTopCut = sketchTopCut.BeginEdit();
            _circleParam = _kompas.GetParamStruct((short)KSConstants.ko_CircleParam);
            _circleParam.xc = 0;
            _circleParam.yc = 0;
            _circleParam.rad = parameters.RadiusBottom - parameters.WallThickness;
            _circleParam.style = 1;
            Document2DTopCut.ksCircle(_circleParam.xc, _circleParam.yc, _circleParam.rad, _circleParam.style);
            sketchTopCut.EndEdit();
            #endregion

            #region Эскиз нижнего основания для вырезания
            ksEntity EntityBotCut = _part.NewEntity((short)KSConstants.o3d_sketch);
            //ksEntity XOYPlaneBotCut = _part.GetDefaultEntity((short)KSConstants.o3d_planeXOY);
            ksSketchDefinition sketchBotCut = EntityBotCut.GetDefinition();
            sketchBotCut.SetPlane(displacedPlaneBotCut);
            EntityBotCut.Create();
            ksDocument2D Document2DBotCut = sketchBotCut.BeginEdit();
            _circleParam = _kompas.GetParamStruct((short)KSConstants.ko_CircleParam);
            _circleParam.xc = 0;
            _circleParam.yc = 0;
            _circleParam.rad = parameters.RadiusTop - parameters.WallThickness;
            _circleParam.style = 1;
            Document2DBotCut.ksCircle(_circleParam.xc, _circleParam.yc, _circleParam.rad, _circleParam.style);
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
                planeOffsetDefinitionHoles.offset = parameters.RadiusBottom + 2;
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
                _circleParam.yc = -10;
                _circleParam.rad = 5;
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
                cutExtrusionDefinition.SetSideParam(true, etBlind, parameters.RadiusBottom * 4, 0, false);
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
                planeOffsetDefinitionStandBeam.offset = parameters.StandHeight - 5;
                planeOffsetDefinitionStandBeam.SetPlane(EntityDisplacedStandBeam);
                displacedPlaneStandBeam.Create();
                #endregion

                #region смещеная плоскость для ножек стойки
                _part = _doc3D.GetPart((short)KSConstants.pTop_part);
                ksEntity EntityDisplacedStandLeg = _part.GetDefaultEntity((short)KSConstants.o3d_planeXOY);

                ksEntity displacedPlaneStandLeg = _part.NewEntity((short)KSConstants.o3d_planeOffset);
                ksPlaneOffsetDefinition planeOffsetDefinitionStandLeg = displacedPlaneStandLeg.GetDefinition();
                planeOffsetDefinitionStandLeg.direction = true;
                planeOffsetDefinitionStandLeg.offset = parameters.StandHeight - 1;
                planeOffsetDefinitionStandLeg.SetPlane(EntityDisplacedStandLeg);
                displacedPlaneStandLeg.Create();
                #endregion

                #region смещеная плоскость для крепления ножек стойки
                _part = _doc3D.GetPart((short)KSConstants.pTop_part);
                ksEntity EntityDisplacedStandBracingLeft = _part.GetDefaultEntity((short)KSConstants.o3d_planeXOY);

                ksEntity displacedPlaneStandBracingLeft = _part.NewEntity((short)KSConstants.o3d_planeOffset);
                ksPlaneOffsetDefinition planeOffsetDefinitionStandBracingLeft = displacedPlaneStandBracingLeft.GetDefinition();
                planeOffsetDefinitionStandBracingLeft.direction = true;
                planeOffsetDefinitionStandBracingLeft.offset = 10;
                planeOffsetDefinitionStandBracingLeft.SetPlane(EntityDisplacedStandBracingLeft);
                displacedPlaneStandBracingLeft.Create();
                #endregion

                #region эскиз для крепления ножек стойки
                ksEntity EntityStandBracing = _part.NewEntity((short)KSConstants.o3d_sketch);
                ksSketchDefinition sketchStandBracing = EntityStandBracing.GetDefinition();
                sketchStandBracing.SetPlane(displacedPlaneStandBracingLeft);
                EntityStandBracing.Create();
                ksDocument2D Document2DStandBracing = sketchStandBracing.BeginEdit();
                _lineParam = _kompas.GetParamStruct((short)KSConstants.ko_LineSegParam);
                _lineParam.x1 = 0;
                _lineParam.x2 = 0.5;
                _lineParam.y1 = parameters.RadiusBottom + 1;
                _lineParam.y2 = parameters.RadiusBottom + 1;
                _lineParam.style = 1;
                Document2DStandBracing.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _circleParam.style);
                _lineParam.x1 = 0.5;
                _lineParam.x2 = 0.5;
                _lineParam.y1 = parameters.RadiusBottom + 1;
                _lineParam.y2 = parameters.RadiusTop - 2;
                _lineParam.style = 1;
                Document2DStandBracing.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _circleParam.style);
                _lineParam.x1 = 0.5;
                _lineParam.x2 = 6.5;
                _lineParam.y1 = parameters.RadiusTop - 2;
                _lineParam.y2 = parameters.RadiusTop - 2;
                _lineParam.style = 1;
                Document2DStandBracing.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _circleParam.style);
                _lineParam.x1 = 6.5;
                _lineParam.x2 = 6.5;
                _lineParam.y1 = parameters.RadiusTop - 2;
                _lineParam.y2 = parameters.RadiusTop - 3;
                _lineParam.style = 1;
                Document2DStandBracing.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _circleParam.style);
                _lineParam.x1 = 6.5;
                _lineParam.x2 = 0.5;
                _lineParam.y1 = parameters.RadiusTop - 3;
                _lineParam.y2 = parameters.RadiusTop - 3;
                _lineParam.style = 1;
                Document2DStandBracing.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _circleParam.style);

                //правая
                _lineParam.x1 = 0;
                _lineParam.x2 = 0.5;
                _lineParam.y1 = -(parameters.RadiusBottom + 1);
                _lineParam.y2 = -(parameters.RadiusBottom + 1);
                _lineParam.style = 1;
                Document2DStandBracing.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _circleParam.style);
                _lineParam.x1 = 0.5;
                _lineParam.x2 = 0.5;
                _lineParam.y1 = -(parameters.RadiusBottom + 1);
                _lineParam.y2 = -(parameters.RadiusTop - 2);
                _lineParam.style = 1;
                Document2DStandBracing.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _circleParam.style);
                _lineParam.x1 = 0.5;
                _lineParam.x2 = 6.5;
                _lineParam.y1 = -(parameters.RadiusTop - 2);
                _lineParam.y2 = -(parameters.RadiusTop - 2);
                _lineParam.style = 1;
                Document2DStandBracing.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _circleParam.style);
                _lineParam.x1 = 6.5;
                _lineParam.x2 = 6.5;
                _lineParam.y1 = -(parameters.RadiusTop - 2);
                _lineParam.y2 = -(parameters.RadiusTop - 3);
                _lineParam.style = 1;
                Document2DStandBracing.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _circleParam.style);
                _lineParam.x1 = 6.5;
                _lineParam.x2 = 0.5;
                _lineParam.y1 = -(parameters.RadiusTop - 3);
                _lineParam.y2 = -(parameters.RadiusTop - 3);
                _lineParam.style = 1;
                Document2DStandBracing.ksLineSeg(_lineParam.x1, _lineParam.y1, _lineParam.x2, _lineParam.y2, _circleParam.style);
                //ось 
                ksAxisLineParam axisLineParam = _kompas.GetParamStruct((short)KSConstants.ko_AxisLineParam);
                ksMathPointParam begpoint = axisLineParam.GetBegPoint();
                ksMathPointParam endpoint = axisLineParam.GetEndPoint();
                begpoint.x = 0;
                begpoint.y = 0;
                endpoint.x = 0;
                endpoint.y = 4;
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
                _rectangleParam.y = parameters.RadiusBottom + 1;
                _rectangleParam.x = parameters.RadiusBottom;
                _rectangleParam.width = -2*(parameters.RadiusBottom + 1);
                _rectangleParam.height = 1;
                _rectangleParam.style = 1;
                Document2DStandStops.ksRectangle(_rectangleParam, 0);
                _rectangleParam.ang = 0;
                _rectangleParam.y = -(parameters.RadiusBottom + 1);
                _rectangleParam.x = -parameters.RadiusBottom;
                _rectangleParam.width = 2*(parameters.RadiusBottom + 1);
                _rectangleParam.height = -1;
                _rectangleParam.style = 1;
                Document2DStandStops.ksRectangle(_rectangleParam, 0);

                sketchStandStops.EndEdit();
                #endregion


            }

        }
    }
}
