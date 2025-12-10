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
            return shapes[0];
        }
        let _union = union({
            "type": "FeatureCollection",
            "features": shapes
        });
        // because union() puts the vertices in the wrong order, causing constituencies to be colored on the outside
        _union!.geometry = rewind(_union!.geometry, {reverse: true}) as Polygon | MultiPolygon;
        return _union;
    });
    let geoCollection = {
        "type": "FeatureCollection",
        "features": constituencies
    };
    return geoCollection;
}

// var geoGenerator: GeoPath<any, GeoPermissibleObjects> | ((arg0: any) => string);

// async function init() {
//     const constituencies = await generateConstituencies();
//     const bboxConstituencies = bbox(constituencies.features);
//     console.log(bboxConstituencies);
//     const projection =
//         geoMercator()
//         .scale(2000) // Do dopracowania jaką wartość dokładnie dobrać
//         .translate([0, 0])
//         .center([bboxConstituencies[0], bboxConstituencies[3]]);

//     geoGenerator = geoPath().projection(projection);

//     console.log(constituencies.features)
//     // const u =
//     //     select('#content g.map')
//     //     .selectAll('path')
//     //     .data(constituencies.features)
//     //     .join('path')
//     //     .attr('d', geoGenerator)
//     //     .attr('fill', "#f00");

//     const u =
//         select('svg.map')
//         .selectAll('a')
//         .data(constituencies.features)
//         .join('a')
//         .attr('id', getTERC)
//         .attr('name', getRegionName)
//         .attr('href', getDistrictURL)
//         .append(getPath);
// }

// init();

// function getRegionName(region: { properties: { name: any; }; }) {
//     return region.properties.name;
// }

// function getTERC(region: { properties: { terc: any; }; }) {
//     return region.properties.terc;
// }

// function getDistrictURL(region: any) {
//     return "https://en.wikipedia.org/wiki/" + getTERC(region);
// }

// function getPath(region: any) {
//     var path = document.createElement('path');
//     path.setAttribute('d', geoGenerator(region));
//     return path;
// }