declare module "geojson-dissolve" {
  import {
    Feature,
    FeatureCollection,
    Polygon,
    MultiPolygon,
    Geometry
  } from "geojson";

  type GeoInput =
    | Feature<Polygon | MultiPolygon>
    | FeatureCollection<Polygon | MultiPolygon>;

  export function dissolve(input: GeoInput): Feature | FeatureCollection;
}