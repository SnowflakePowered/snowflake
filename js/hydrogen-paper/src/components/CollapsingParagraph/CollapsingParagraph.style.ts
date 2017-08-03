
const LINE_HEIGHT = 1.5

export const styles = {
  expand: {
    transform: 'rotate(0deg)'
  },
  expandOpen: {
    transform: 'rotate(180deg)'
  },
  container: {
    display: 'grid',
    gridTemplateColumns: '[textSection] auto [toolbar] min-content'
  },
  textSection: {
    gridColumn: 'textSection'
  },
  toolbar: {
    gridColumn: 'toolbar'
  },
  collapse: {
    // Override the MUI Collapse to show the first line of the text.
    // 1em * 1.5 value is given by (
    // (relative line-height) = (absolute line-height) / (font-size) =>
    // (absolute line-height) = (relative line-height) * (font-size).
    height: `calc(1em * ${LINE_HEIGHT}) !important`
  },
  paragraph: {
    lineHeight: LINE_HEIGHT
  }
}
