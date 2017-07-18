import React from 'react'
import BooleanWidget from 'components/configuration/BooleanWidget'

const ConfigSectionView = ({sectionName, sectionContents}) => {
  return (
    <div>
      {sectionName}
      {
        Object.keys(sectionContents.Values).map(p => ({
          Key: p,
          Value: sectionContents.Values[p],
          Option: sectionContents.Options[p],
          Selection: sectionContents.Selections[p]
        })).map(c => <ConfigOptionView {...c}/>)
      }
    </div>
  )
}

const ConfigOptionView = ({Key, Value, Option, Selection}) => {
  if (Option.Type === 'boolean') {
    return <BooleanWidget name={Key} value={Value.Value} description="testing"/>
  }
  return (<div>{Key}</div>)
}

// Object.keys(config.Values).map(p => })
class ConfigurationView extends React.Component {
  render () {
    console.log(this.props.config)
    return (
      <div>
        {(Object.entries(this.props.config).map(
          configSection => <ConfigSectionView
            key={configSection[0]}
            sectionName={configSection[0]}
            sectionContents={configSection[1]} />
        ))}
      </div>
    )
  }
}

export default ConfigurationView
