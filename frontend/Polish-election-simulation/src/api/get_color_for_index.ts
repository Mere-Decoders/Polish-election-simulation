export default function get_color_for_index(colorIndex: number, maxColorIndex: number) {
  if (colorIndex === -1) {
    return 'hsl(0, 0%, 0%)'
  }

  console.log(get_color_for_index);

  //const hue = ((((colorIndex + 1) * get_color_for_index.interval) % get_color_for_index.upperBound) / get_color_for_index.upperBound) * 360
  let hue = 0
  for (let i = 0; i < colorIndex; i++)
  	hue = nextColor(hue)

  console.log(hue)

  return `hsl(${hue * 360}, 100%, 50%)`
}

const GOLDEN_RATIO = 0.61803398875

function nextColor(hue) {
  hue = (hue + GOLDEN_RATIO) % 1
  return hue
}

