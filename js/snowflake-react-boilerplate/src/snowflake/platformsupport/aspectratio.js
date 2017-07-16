export const ASPECT_RATIOS = {
  PORTRAIT: { portrait: true, landscape: false, square: false },
  LANDSCAPE: { portrait: false, landscape: true, square: false },
  SQUARE: { portrait: false, landscape: false, square: true }
}

const platformAspectMapping = {
  NINTENDO_NES: ASPECT_RATIOS.PORTRAIT,
  NINTENDO_SNES: ASPECT_RATIOS.LANDSCAPE
}

export default platformAspectMapping
