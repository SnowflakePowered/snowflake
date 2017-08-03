import grey from 'material-ui/colors/grey'
import common from 'material-ui/colors/common'

export const styles = {
  button: {
    borderRadius: '50%',
    width: '100%',
    height: '100%',
    padding: 8,
    display: 'inline-flex',
    justifyContent: 'center',
    boxSizing: 'border-box',
    overflow: 'hidden',
    backgroundColor: common.white
  },
  arrow: {
    color: grey[800],
    height: 24,
    width: 24
  },
  buttonContainer: {
    height: 40,
    width: 40,
    display: 'block',
    position: 'relative',
    zIndex: 100
  },
  pulse: {
    '$buttonContainer:hover &': {
      transform: 'scale(1.15)'
    },
    position: 'absolute',
    top: 0,
    bottom: 0,
    width: '100%',
    height: '100%',
    borderRadius: '50%',
    background: 'rgba(0,0,0,0.34)',
    zIndex: -1,
    transform: 'scale(1.05)',
    transition: 'transform 0.2s ease'
  },
  progress: {
    color: grey[500],
    position: 'absolute',
    top: 0,
    left: 0,
    zIndex: 10
  }
}
