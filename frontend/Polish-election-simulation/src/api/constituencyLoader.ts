import { union } from '@turf/union'
import { rewind } from "@turf/rewind";
import type { Polygon, MultiPolygon } from 'geojson';

export async function loadPowiaty() {
  const response = await fetch('/poland.counties.json');
  return response.json();
}

export async function loadConstituencies() {
  const response = await fetch('/constituencies.json');
  return response.json();
}

export async function generateConstituencies() {
    const powiaty = await loadPowiaty();
    const constituencyDef = await loadConstituencies();
    let constituencies = constituencyDef.features.map(function(row: { num: string; powiaty: Array<string>; }) {
        let shapes = powiaty.features.filter(function(item: { properties: { terc: string; }; }) {
            return row.powiaty.includes(item.properties.terc);
        });
        if (shapes.length === 1) {
            shapes[0].id = row.num;
            return shapes[0];
        }
        let _union = union({
            "type": "FeatureCollection",
            "features": shapes
        });
        // because union() puts the vertices in the wrong order, causing constituencies to be colored on the outside
        _union!.geometry = rewind(_union!.geometry, {reverse: true}) as Polygon | MultiPolygon;
        _union!.id = row.num;
        return _union;
    });
    let geoCollection = {
        "type": "FeatureCollection",
        "features": constituencies
    };
    return geoCollection;
}