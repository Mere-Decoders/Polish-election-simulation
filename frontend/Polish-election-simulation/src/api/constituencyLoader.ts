import { select, geoPath, geoMercator, type GeoPath, type GeoPermissibleObjects } from 'd3';
import { bbox } from '@turf/bbox'
import { union } from '@turf/union'

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
    console.log(constituencyDef.features);
    let constituencies = constituencyDef.features.map(function(row: { num: string; powiaty: Array<string>; }) {
        let shapes = powiaty.features.filter(function(item: { properties: { terc: string; }; }) {
            return row.powiaty.includes(item.properties.terc);
        });
        let shape = union({
            "type": "FeatureCollection",
            "features": shapes
        });
        console.log("shape");
        console.log(shape);
        return shape;
    });
    let geoCollection = {
        "type": "FeatureCollection",
        "features": constituencies
    };
    console.log(geoCollection);
    return geoCollection;
}

// var geoGenerator: GeoPath<any, GeoPermissibleObjects> | ((arg0: any) => string);

// async function init() {
//     const powiaty = await loadPowiaty();
//     const bboxPowiaty = bbox(powiaty);
//     console.log(bboxPowiaty);
//     const projection =
//         geoMercator()
//         .scale(2000) // Do dopracowania jaką wartość dokładnie dobrać
//         .translate([0, 0])
//         .center([bboxPowiaty[0], bboxPowiaty[3]]);

//     geoGenerator = geoPath().projection(projection);

//     console.log(powiaty.features)
//     // const u =
//     //     select('#content g.map')
//     //     .selectAll('path')
//     //     .data(powiaty.features)
//     //     .join('path')
//     //     .attr('d', geoGenerator)
//     //     .attr('fill', "#f00");

//     const u =
//         select('svg.map')
//         .selectAll('a')
//         .data(powiaty.features)
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