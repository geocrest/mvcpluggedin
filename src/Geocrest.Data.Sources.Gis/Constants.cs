namespace Geocrest.Data.Sources.Gis.Constants
{
    //
    // TODO: Add Image And Network servers
    //

    #region Base Classes
    /// <summary>
    /// Provides the base class for classes that expose constants available in the ArcGIS Server 10.0 REST API. 
    /// </summary>
    public class ArcGISConstantBase
    {
        #region Public Constants
        /// <summary>
        /// Provides the name of the f (format) parameter available with all requests.
        /// </summary>
        public const string f = "f";
        #endregion
    }

    /// <summary>
    /// Provides the base class for classes that expose query operation constants available in the ArcGIS Server 10.0 REST API. 
    /// </summary>
    public class ArcGISQueryConstantBase : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
    {
        #region Public Constants
        /// <summary>
        /// Provides the name of the objectIds parameter.
        /// </summary>
        public const string objectIds = "objectIds";
        /// <summary>
        /// Provides the name of the where parameter.
        /// </summary>
        public const string where = "where";
        /// <summary>
        /// Provides the name of the geometry parameter.
        /// </summary>
        public const string geometry = "geometry";
        /// <summary>
        /// Provides the name of the geometryType parameter.
        /// </summary>
        public const string geometryType = "geometryType";
        /// <summary>
        /// Provides the name of the inSR parameter.
        /// </summary>
        public const string inSR = "inSR";
        /// <summary>
        /// Provides the name of the spatialRel parameter.
        /// </summary>
        public const string spatialRel = "spatialRel";
        /// <summary>
        /// Provides the name of the relationParam parameter.
        /// </summary>
        public const string relationParam = "relationParam";
        /// <summary>
        /// Provides the name of the time parameter.
        /// </summary>
        public const string time = "time";
        /// <summary>
        /// Provides the name of the outFields parameter.
        /// </summary>
        public const string outFields = "outFields";
        /// <summary>
        /// Provides the name of the returnGeometry parameter.
        /// </summary>
        public const string returnGeometry = "returnGeometry";
        /// <summary>
        /// Provides the name of the outSR parameter.
        /// </summary>
        public const string outSR = "outSR";
        /// <summary>
        /// Provides the name of the returnIdsOnly parameter.
        /// </summary>
        public const string returnIdsOnly = "returnIdsOnly";
        /// <summary>
        /// Provides the name of the returnCountOnly parameter.
        /// </summary>
        public const string returnCountOnly = "returnCountOnly";
        #endregion
    }
    #endregion

    /// <summary>
    /// Represents map server operations and parameters available in the ArcGIS Server 10.0 REST API.
    /// </summary>
    public class MAP
    {
        #region Public Constants
        /// <summary>
        /// Provides the name of the exportMap operation.
        /// </summary>
        public const string ExportMap = "exportMap";

        /// <summary>
        /// Provides the name of the identify operation.
        /// </summary>
        public const string Identify = "identify";

        /// <summary>
        /// Provides the name of the find operation.
        /// </summary>
        public const string Find = "find";

        /// <summary>
        /// Provides the name of the generateKml operation.
        /// </summary>
        public const string GenerateKml = "generateKml";

        /// <summary>
        /// Provides the name of the query operation used on layer and table resources.
        /// </summary>
        public const string LayerOrTableQuery = "query";

        /// <summary>
        /// Provides the name of the queryRelatedRecords operation used on layer and table resources.
        /// </summary>
        public const string LayerOrTableQueryRelatedRecords = "queryRelatedRecords";
        #endregion

        #region Embedded Classes
        /// <summary>
        /// Provides names of parameters used by the exportMap operation.
        /// </summary>
        public class ExportMapParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the bbox parameter.
            /// </summary>
            public const string bbox = "bbox";

            /// <summary>
            /// Provides the name of the size parameter.
            /// </summary>
            public const string size = "size";

            /// <summary>
            /// Provides the name of the dpi parameter.
            /// </summary>
            public const string dpi = "dpi";

            /// <summary>
            /// Provides the name of the imageSR parameter.
            /// </summary>
            public const string imageSR = "imageSR";

            /// <summary>
            /// Provides the name of the bboxSR parameter.
            /// </summary>
            public const string bboxSR = "bboxSR";

            /// <summary>
            /// Provides the name of the format parameter.
            /// </summary>
            public const string format = "format";

            /// <summary>
            /// Provides the name of the layerDefs parameter.
            /// </summary>
            public const string layerDefs = "layerDefs";

            /// <summary>
            /// Provides the name of the layers parameter.
            /// </summary>
            public const string layers = "layers";

            /// <summary>
            /// Provides the name of the transparent parameter.
            /// </summary>
            public const string transparent = "transparent";

            /// <summary>
            /// Provides the name of the time parameter.
            /// </summary>
            public const string time = "time";

            /// <summary>
            /// Provides the name of the layerTimeOptions parameter.
            /// </summary>
            public const string layerTimeOptions = "layerTimeOptions";
            #endregion
        }

        /// <summary>
        /// Provides names of parameters used by the identify operation.
        /// </summary>
        public class IdentifyParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the geometry parameter.
            /// </summary>
            public const string geometry = "geometry";

            /// <summary>
            /// Provides the name of the geometryType parameter.
            /// </summary>
            public const string geometryType = "geometryType";

            /// <summary>
            /// Provides the name of the sr parameter.
            /// </summary>
            public const string sr = "sr";

            /// <summary>
            /// Provides the name of the layerDefs parameter.
            /// </summary>
            public const string layerDefs = "layerDefs";

            /// <summary>
            /// Provides the name of the time parameter.
            /// </summary>
            public const string time = "time";

            /// <summary>
            /// Provides the name of the layerTimeOptions parameter.
            /// </summary>
            public const string layerTimeOptions = "layerTimeOptions";

            /// <summary>
            /// Provides the name of the layers parameter.
            /// </summary>
            public const string layers = "layers";

            /// <summary>
            /// Provides the name of the tolerance parameter.
            /// </summary>
            public const string tolerance = "tolerance";

            /// <summary>
            /// Provides the name of the mapExtent parameter.
            /// </summary>
            public const string mapExtent = "mapExtent";

            /// <summary>
            /// Provides the name of the imageDisplay parameter.
            /// </summary>
            public const string imageDisplay = "imageDisplay";

            /// <summary>
            /// Provides the name of the returnGeometry parameter.
            /// </summary>
            public const string returnGeometry = "returnGeometry";

            /// <summary>
            /// Provides the name of the maxAllowableOffset parameter.
            /// </summary>
            public const string maxAllowableOffset = "maxAllowableOffset";
            #endregion
        }

        /// <summary>
        /// Provides names of parameters used by the find operation.
        /// </summary>
        public class FindParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the searchText parameter.
            /// </summary>
            public const string searchText = "searchText";

            /// <summary>
            /// Provides the name of the contains parameter.
            /// </summary>
            public const string contains = "contains";

            /// <summary>
            /// Provides the name of the searchFields parameter.
            /// </summary>
            public const string searchFields = "searchFields";

            /// <summary>
            /// Provides the name of the sr parameter.
            /// </summary>
            public const string sr = "sr";

            /// <summary>
            /// Provides the name of the layerDefs parameter.
            /// </summary>
            public const string layerDefs = "layerDefs";

            /// <summary>
            /// Provides the name of the layers parameter.
            /// </summary>
            public const string layers = "layers";

            /// <summary>
            /// Provides the name of the returnGeometry parameter.
            /// </summary>
            public const string returnGeometry = "returnGeometry";

            /// <summary>
            /// Provides the name of the maxAllowableOffset parameter.
            /// </summary>
            public const string maxAllowableOffset = "maxAllowableOffset";
            #endregion
        }

        /// <summary>
        /// Provides names of parameters used by the generateKml operation.
        /// </summary>
        public class GenerateKmlParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the docName parameter.
            /// </summary>
            public const string docName = "docName";

            /// <summary>
            /// Provides the name of the layers parameter.
            /// </summary>
            public const string layers = "layers";

            /// <summary>
            /// Provides the name of the layerOptions parameter.
            /// </summary>
            public const string layerOptions = "layerOptions";
            #endregion
        }

        /// <summary>
        /// Provides names of parameters used by the query operation on layer and table resources.
        /// </summary>
        public class LayerOrTableQueryParam : Geocrest.Data.Sources.Gis.Constants.ArcGISQueryConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the text parameter.
            /// </summary>
            public const string text = "text";

            /// <summary>
            /// Provides the name of the maxAllowableOffset parameter.
            /// </summary>
            public const string maxAllowableOffset = "maxAllowableOffset";
            #endregion
        }

        /// <summary>
        /// Provides names of parameters used by the queryRelatedRecords operation on layer and table resources.
        /// </summary>
        public class LayerOrTableQueryRelatedRecordsParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the objectIds parameter.
            /// </summary>
            public const string objectIds = "objectIds";

            /// <summary>
            /// Provides the name of the relationshipId parameter.
            /// </summary>
            public const string relationshipId = "relationshipId";

            /// <summary>
            /// Provides the name of the outFields parameter.
            /// </summary>
            public const string outFields = "outFields";

            /// <summary>
            /// Provides the name of the definitionExpression parameter.
            /// </summary>
            public const string definitionExpression = "definitionExpression";

            /// <summary>
            /// Provides the name of the returnGeometry parameter.
            /// </summary>
            public const string returnGeometry = "returnGeometry";

            /// <summary>
            /// Provides the name of the maxAllowableOffset parameter.
            /// </summary>
            public const string maxAllowableOffset = "maxAllowableOffset";

            /// <summary>
            /// Provides the name of the outSR parameter.
            /// </summary>
            public const string outSR = "outSR";
            #endregion
        }
        #endregion
    }

    /// <summary>
    /// Represents geocode server operations and parameters available in the ArcGIS Server 10.0 REST API.
    /// </summary>
    public class GEOCODE
    {
        #region Public Constants
        /// <summary>
        /// Provides the name of the findAddressCandidates operation.
        /// </summary>
        public const string FindAddressCandidates = "findAddressCandidates";

        /// <summary>
        /// Provides the name of the reverseGeocode operation.
        /// </summary>
        public const string ReverseGeocode = "reverseGeocode";
        #endregion

        #region Embedded Classes
        /// <summary>
        /// Provides names of parameters used by the findAddressCandidates operation.
        /// </summary>
        public class FindAddressCandidatesParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            /// <summary>
            /// Provides the name of the outFields parameter.
            /// </summary>
            public const string outFields = "outFields";

            /// <summary>
            /// Provides the name of the outSR parameter.
            /// </summary>
            public const string outSR = "outSR";
        }

        /// <summary>
        /// Provides names of parameters used by the reverseGeocode operation.
        /// </summary>
        public class ReverseGeocodeParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            /// <summary>
            /// Provides the name of the location parameter.
            /// </summary>
            public const string location = "location";

            /// <summary>
            /// Provides the name of the distance parameter.
            /// </summary>
            public const string distance = "distance";

            /// <summary>
            /// Provides the name of the outSR parameter.
            /// </summary>
            public const string outSR = "outSR";
        }
        #endregion
    }

    /// <summary>
    /// Represents geoprocessing server operations and parameters available in the ArcGIS Server 10.0 REST API.
    /// </summary>
    public class GP
    {
        #region Public Constants
        /// <summary>
        /// Provides the name of the executeGPTask operation.
        /// </summary>
        public const string ExecuteGPTask = "executeGPTask";

        /// <summary>
        /// Provides the name of the submitJob operation.
        /// </summary>
        public const string SubmitJob = "submitJob";
        #endregion

        #region Embedded Classes
        /// <summary>
        /// Provides names of parameters used by the executeGPTask operation.
        /// </summary>
        public class ExecuteGPTaskParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the env:outSR parameter.
            /// </summary>
            public const string env_outSR = "env:outSR";

            /// <summary>
            /// Provides the name of the env:processSR parameter.
            /// </summary>
            public const string env_processSR = "env:processSR";
            #endregion
        }

        /// <summary>
        /// Provides names of parameters used by the submitJob operation.
        /// </summary>
        public class SubmitJobParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the env:outSR parameter.
            /// </summary>
            public const string env_outSR = "env:outSR";

            /// <summary>
            /// Provides the name of the env:processSR parameter.
            /// </summary>
            public const string env_processSR = "env:processSR";
            #endregion
        }
        #endregion
    }

    /// <summary>
    /// Represents geometry server operations and parameters available in the ArcGIS Server 10.0 REST API.
    /// </summary>
    public class GEOMETRY
    {
        #region Public Constants
        /// <summary>
        /// Provides the name of the project operation.
        /// </summary>
        public const string Project = "project";

        /// <summary>
        /// Provides the name of the simplify operation.
        /// </summary>
        public const string Simplify = "simplify";

        /// <summary>
        /// Provides the name of the bufferGeometries operation. NOTE: this may need to be changed to simply "buffer".
        /// </summary>
        public const string BufferGeometries = "bufferGeometries";

        /// <summary>
        /// Provides the name of the areasAndLengths operation.
        /// </summary>
        public const string AreasAndLengths = "areasAndLengths";

        /// <summary>
        /// Provides the name of the lengths operation.
        /// </summary>
        public const string Lengths = "lengths";

        /// <summary>
        /// Provides the name of the relation operation.
        /// </summary>
        public const string Relation = "relation";

        /// <summary>
        /// Provides the name of the labelPoints operation.
        /// </summary>
        public const string LabelPoints = "labelPoints";

        /// <summary>
        /// Provides the name of the distance operation.
        /// </summary>
        public const string Distance = "distance";

        /// <summary>
        /// Provides the name of the densify operation.
        /// </summary>
        public const string Densify = "densify";

        /// <summary>
        /// Provides the name of the generalize operation.
        /// </summary>
        public const string Generalize = "generalize";

        /// <summary>
        /// Provides the name of the convexHull operation.
        /// </summary>
        public const string ConvexHull = "convexHull";

        /// <summary>
        /// Provides the name of the offset operation.
        /// </summary>
        public const string Offset = "offset";

        /// <summary>
        /// Provides the name of the trimExtend operation.
        /// </summary>
        public const string TrimExtend = "trimExtend";

        /// <summary>
        /// Provides the name of the autoComplete operation.
        /// </summary>
        public const string AutoComplete = "autoComplete";

        /// <summary>
        /// Provides the name of the cut operation.
        /// </summary>
        public const string Cut = "cut";

        /// <summary>
        /// Provides the name of the difference operation.
        /// </summary>
        public const string Difference = "difference";

        /// <summary>
        /// Provides the name of the intersect operation.
        /// </summary>
        public const string Intersect = "intersect";

        /// <summary>
        /// Provides the name of the reshape operation.
        /// </summary>
        public const string Reshape = "reshape";

        /// <summary>
        /// Provides the name of the union operation.
        /// </summary>
        public const string Union = "union";
        #endregion

        #region Embedded Classes
        /// <summary>
        /// Provides names of parameters used by the projectGeometries operation.
        /// </summary>
        public class ProjectGeometriesParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the geometries parameter.
            /// </summary>
            public const string geometries = "geometries";

            /// <summary>
            /// Provides the name of the inSR parameter.
            /// </summary>
            public const string inSR = "inSR";

            /// <summary>
            /// Provides the name of the outSR parameter.
            /// </summary>
            public const string outSR = "outSR";
            #endregion
        }

        /// <summary>
        /// Provides names of parameters used by the simplify operation.
        /// </summary>
        public class SimplifyParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the geometries parameter.
            /// </summary>
            public const string geometries = "geometries";

            /// <summary>
            /// Provides the name of the sr parameter.
            /// </summary>
            public const string sr = "sr";
            #endregion
        }

        /// <summary>
        /// Provides names of parameters used by the bufferGeometries operation.
        /// </summary>
        public class BufferGeometriesParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the geometries parameter.
            /// </summary>
            public const string geometries = "geometries";

            /// <summary>
            /// Provides the name of the inSR parameter.
            /// </summary>
            public const string inSR = "inSR";

            /// <summary>
            /// Provides the name of the outSR parameter.
            /// </summary>
            public const string outSR = "outSR";

            /// <summary>
            /// Provides the name of the bufferSR parameter.
            /// </summary>
            public const string bufferSR = "bufferSR";

            /// <summary>
            /// Provides the name of the distances parameter.
            /// </summary>
            public const string distances = "distances";

            /// <summary>
            /// Provides the name of the unit parameter.
            /// </summary>
            public const string unit = "unit";

            /// <summary>
            /// Provides the name of the unionResults parameter.
            /// </summary>
            public const string unionResults = "unionResults";
            #endregion
        }

        /// <summary>
        /// Provides names of parameters used by the areasAndLengths operation.
        /// </summary>
        public class AreasAndLengthsParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the polygons parameter.
            /// </summary>
            public const string polygons = "polygons";

            /// <summary>
            /// Provides the name of the sr parameter.
            /// </summary>
            public const string sr = "sr";

            /// <summary>
            /// Provides the name of the lengthUnit parameter.
            /// </summary>
            public const string lengthUnit = "lengthUnit";

            /// <summary>
            /// Provides the name of the areaUnit parameter.
            /// </summary>
            public const string areaUnit = "areaUnit";
            #endregion
        }

        /// <summary>
        /// Provides names of parameters used by the lengths operation.
        /// </summary>
        public class LengthsParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the polylines parameter.
            /// </summary>
            public const string polylines = "polylines";

            /// <summary>
            /// Provides the name of the sr parameter.
            /// </summary>
            public const string sr = "sr";

            /// <summary>
            /// Provides the name of the lengthUnit parameter.
            /// </summary>
            public const string lengthUnit = "lengthUnit";

            /// <summary>
            /// Provides the name of the geodesic parameter.
            /// </summary>
            public const string geodesic = "geodesic";
            #endregion
        }

        /// <summary>
        /// Provides names of parameters used by the relation operation.
        /// </summary>
        public class RelationParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the geometries1 parameter.
            /// </summary>
            public const string geometries1 = "geometries1";

            /// <summary>
            /// Provides the name of the geometries2 parameter.
            /// </summary>
            public const string geometries2 = "geometries2";

            /// <summary>
            /// Provides the name of the sr parameter.
            /// </summary>
            public const string sr = "sr";

            /// <summary>
            /// Provides the name of the relation parameter.
            /// </summary>
            public const string relation = "relation";

            /// <summary>
            /// Provides the name of the relationParam parameter.
            /// </summary>
            public const string relationParam = "relationParam";
            #endregion
        }

        /// <summary>
        /// Provides names of parameters used by the labelPoints operation.
        /// </summary>
        public class LabelPointsParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the polygons parameter.
            /// </summary>
            public const string polygons = "polygons";

            /// <summary>
            /// Provides the name of the sr parameter.
            /// </summary>
            public const string sr = "sr";
            #endregion
        }

        /// <summary>
        /// Provides names of parameters used by the distance operation.
        /// </summary>
        public class DistanceParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the geometry1 parameter.
            /// </summary>
            public const string geometry1 = "geometry1";

            /// <summary>
            /// Provides the name of the geometry2 parameter.
            /// </summary>
            public const string geometry2 = "geometry2";

            /// <summary>
            /// Provides the name of the sr parameter.
            /// </summary>
            public const string sr = "sr";

            /// <summary>
            /// Provides the name of the distanceUnit parameter.
            /// </summary>
            public const string distanceUnit = "distanceUnit";

            /// <summary>
            /// Provides the name of the geodesic parameter.
            /// </summary>
            public const string geodesic = "geodesic";
            #endregion
        }

        /// <summary>
        /// Provides names of parameters used by the densify operation.
        /// </summary>
        public class DensifyParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the geometries parameter.
            /// </summary>
            public const string geometries = "geometries";

            /// <summary>
            /// Provides the name of the sr parameter.
            /// </summary>
            public const string sr = "sr";

            /// <summary>
            /// Provides the name of the maxSegmentLength parameter.
            /// </summary>
            public const string maxSegmentLength = "maxSegmentLength";

            /// <summary>
            /// Provides the name of the geodesic parameter.
            /// </summary>
            public const string geodesic = "geodesic";

            /// <summary>
            /// Provides the name of the lengthUnit parameter.
            /// </summary>
            public const string lengthUnit = "lengthUnit";
            #endregion
        }

        /// <summary>
        /// Provides names of parameters used by the generalize operation.
        /// </summary>
        public class GeneralizeParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the geometries parameter.
            /// </summary>
            public const string geometries = "geometries";

            /// <summary>
            /// Provides the name of the sr parameter.
            /// </summary>
            public const string sr = "sr";

            /// <summary>
            /// Provides the name of the maxDeviation parameter.
            /// </summary>
            public const string maxDeviation = "maxDeviation";

            /// <summary>
            /// Provides the name of the deviationUnit parameter.
            /// </summary>
            public const string deviationUnit = "deviationUnit";
            #endregion
        }

        /// <summary>
        /// Provides names of parameters used by the convexHull operation.
        /// </summary>
        public class ConvexHullParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the geometries parameter.
            /// </summary>
            public const string geometries = "geometries";

            /// <summary>
            /// Provides the name of the sr parameter.
            /// </summary>
            public const string sr = "sr";
            #endregion
        }

        /// <summary>
        /// Provides names of parameters used by the offset operation.
        /// </summary>
        public class OffsetParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the geometries parameter.
            /// </summary>
            public const string geometries = "geometries";

            /// <summary>
            /// Provides the name of the sr parameter.
            /// </summary>
            public const string sr = "sr";

            /// <summary>
            /// Provides the name of the offsetDistance parameter.
            /// </summary>
            public const string offsetDistance = "offsetDistance";

            /// <summary>
            /// Provides the name of the offsetUnit parameter.
            /// </summary>
            public const string offsetUnit = "offsetUnit";

            /// <summary>
            /// Provides the name of the offsetHow parameter.
            /// </summary>
            public const string offsetHow = "offsetHow";

            /// <summary>
            /// Provides the name of the bevelRatio parameter.
            /// </summary>
            public const string bevelRatio = "bevelRatio";
            #endregion
        }

        /// <summary>
        /// Provides names of parameters used by the trimExtend operation.
        /// </summary>
        public class TrimExtendParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the polylines parameter.
            /// </summary>
            public const string polylines = "polylines";

            /// <summary>
            /// Provides the name of the trimExtendTo parameter.
            /// </summary>
            public const string trimExtendTo = "trimExtendTo";

            /// <summary>
            /// Provides the name of the sr parameter.
            /// </summary>
            public const string sr = "sr";

            /// <summary>
            /// Provides the name of the extendHow parameter.
            /// </summary>
            public const string extendHow = "extendHow";
            #endregion
        }

        /// <summary>
        /// Provides names of parameters used by the autoComplete operation.
        /// </summary>
        public class AutoCompleteParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the polygons parameter.
            /// </summary>
            public const string polygons = "polygons";

            /// <summary>
            /// Provides the name of the polylines parameter.
            /// </summary>
            public const string polylines = "polylines";

            /// <summary>
            /// Provides the name of the sr parameter.
            /// </summary>
            public const string sr = "sr";
            #endregion
        }

        /// <summary>
        /// Provides names of parameters used by the cut operation.
        /// </summary>
        public class CutParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the cutter parameter.
            /// </summary>
            public const string cutter = "cutter";

            /// <summary>
            /// Provides the name of the target parameter.
            /// </summary>
            public const string target = "target";

            /// <summary>
            /// Provides the name of the sr parameter.
            /// </summary>
            public const string sr = "sr";
            #endregion
        }

        /// <summary>
        /// Provides names of parameters used by the difference operation.
        /// </summary>
        public class DifferenceParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the geometries parameter.
            /// </summary>
            public const string geometries = "geometries";

            /// <summary>
            /// Provides the name of the geometry parameter.
            /// </summary>
            public const string geometry = "geometry";

            /// <summary>
            /// Provides the name of the sr parameter.
            /// </summary>
            public const string sr = "sr";
            #endregion
        }

        /// <summary>
        /// Provides names of parameters used by the intersect operation.
        /// </summary>
        public class IntersectParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the geometries parameter.
            /// </summary>
            public const string geometries = "geometries";

            /// <summary>
            /// Provides the name of the geometry parameter.
            /// </summary>
            public const string geometry = "geometry";

            /// <summary>
            /// Provides the name of the sr parameter.
            /// </summary>
            public const string sr = "sr";
            #endregion
        }

        /// <summary>
        /// Provides names of parameters used by the reshape operation.
        /// </summary>
        public class ReshapeParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the target parameter.
            /// </summary>
            public const string target = "target";

            /// <summary>
            /// Provides the name of the reshaper parameter.
            /// </summary>
            public const string reshaper = "reshaper";

            /// <summary>
            /// Provides the name of the sr parameter.
            /// </summary>
            public const string sr = "sr";
            #endregion
        }

        /// <summary>
        /// Provides names of parameters used by the union operation.
        /// </summary>
        public class UnionParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the geometries parameter.
            /// </summary>
            public const string geometries = "geometries";

            /// <summary>
            /// Provides the name of the sr parameter.
            /// </summary>
            public const string sr = "sr";
            #endregion
        }
        #endregion
    }

    /// <summary>
    /// Represents feature server operations and parameters available in the ArcGIS Server 10.0 REST API.
    /// </summary>
    public class FEATURE
    {
        #region Public Constants
        /// <summary>
        /// Provides the name of the query operation.
        /// </summary>
        public const string Query = "query";

        /// <summary>
        /// Provides the name of the queryRelatedRecords operation.
        /// </summary>
        public const string QueryRelatedRecords = "queryRelatedRecords";

        /// <summary>
        /// Provides the name of the addFeatures operation.
        /// </summary>
        public const string AddFeatures = "addFeatures";

        /// <summary>
        /// Provides the name of the updateFeatures operation.
        /// </summary>
        public const string UpdateFeatures = "updateFeatures";

        /// <summary>
        /// Provides the name of the deleteFeatures operation.
        /// </summary>
        public const string DeleteFeatures = "deleteFeatures";

        /// <summary>
        /// Provides the name of the applyEdits operation.
        /// </summary>
        public const string ApplyEdits = "applyEdits";

        /// <summary>
        /// Provides the name of the addAttachment operation.
        /// </summary>
        public const string AddAttachment = "addAttachment";

        /// <summary>
        /// Provides the name of the updateAttachment operation.
        /// </summary>
        public const string UpdateAttachment = "updateAttachment";

        /// <summary>
        /// Provides the name of the deleteAttachments operation.
        /// </summary>
        public const string DeleteAttachments = "deleteAttachments";
        #endregion

        #region Embedded Classes
        /// <summary>
        /// Provides names of parameters used by the query operation.
        /// </summary>
        public class QueryParam : Geocrest.Data.Sources.Gis.Constants.ArcGISQueryConstantBase { }

        /// <summary>
        /// Provides names of parameters used by the queryRelatedRecords operation.
        /// </summary>
        public class QueryRelatedRecordsParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the objectIds parameter.
            /// </summary>
            public const string objectIds = "objectIds";

            /// <summary>
            /// Provides the name of the relationshipId parameter.
            /// </summary>
            public const string relationshipId = "relationshipId";

            /// <summary>
            /// Provides the name of the outFields parameter.
            /// </summary>
            public const string outFields = "outFields";

            /// <summary>
            /// Provides the name of the definitionExpression parameter.
            /// </summary>
            public const string definitionExpression = "definitionExpression";

            /// <summary>
            /// Provides the name of the returnGeometry parameter.
            /// </summary>
            public const string returnGeometry = "returnGeometry";

            /// <summary>
            /// Provides the name of the outSR parameter.
            /// </summary>
            public const string outSR = "outSR";
            #endregion
        }

        /// <summary>
        /// Provides names of parameters used by the addFeatures operation.
        /// </summary>
        public class AddFeaturesParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the features parameter.
            /// </summary>
            public const string features = "features";
            #endregion
        }

        /// <summary>
        /// Provides names of parameters used by the updateFeatures operation.
        /// </summary>
        public class UpdateFeaturesParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the features parameter.
            /// </summary>
            public const string features = "features";
            #endregion
        }

        /// <summary>
        /// Provides names of parameters used by the deleteFeatures operation.
        /// </summary>
        public class DeleteFeaturesParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the objectIds parameter.
            /// </summary>
            public const string objectIds = "objectIds";

            /// <summary>
            /// Provides the name of the where parameter.
            /// </summary>
            public const string where = "where";

            /// <summary>
            /// Provides the name of the geometry parameter.
            /// </summary>
            public const string geometry = "geometry";

            /// <summary>
            /// Provides the name of the geometryType parameter.
            /// </summary>
            public const string geometryType = "geometryType";

            /// <summary>
            /// Provides the name of the inSR parameter.
            /// </summary>
            public const string inSR = "inSR";

            /// <summary>
            /// Provides the name of the spatialRel parameter.
            /// </summary>
            public const string spatialRel = "spatialRel";
            #endregion
        }

        /// <summary>
        /// Provides names of parameters used by the applyEdits operation.
        /// </summary>
        public class ApplyEditsParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the adds parameter.
            /// </summary>
            public const string adds = "adds";

            /// <summary>
            /// Provides the name of the updates parameter.
            /// </summary>
            public const string updates = "updates";

            /// <summary>
            /// Provides the name of the deletes parameter.
            /// </summary>
            public const string deletes = "deletes";
            #endregion
        }

        /// <summary>
        /// Provides names of parameters used by the addAttachment operation.
        /// </summary>
        public class AddAttachmentParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the attachment parameter.
            /// </summary>
            public const string attachment = "attachment";
            #endregion
        }

        /// <summary>
        /// Provides names of parameters used by the updateAttachment operation.
        /// </summary>
        public class UpdateAttachmentParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the attachmentId parameter.
            /// </summary>
            public const string attachmentId = "attachmentId";

            /// <summary>
            /// Provides the name of the attachment parameter.
            /// </summary>
            public const string attachment = "attachment";
            #endregion
        }

        /// <summary>
        /// Provides names of parameters used by the deleteAttachments operation.
        /// </summary>
        public class DeleteAttachmentsParam : Geocrest.Data.Sources.Gis.Constants.ArcGISConstantBase
        {
            #region Public Constants
            /// <summary>
            /// Provides the name of the attachmentIds parameter.
            /// </summary>
            public const string attachmentIds = "attachmentIds";
            #endregion
        }
        #endregion
    }
}
