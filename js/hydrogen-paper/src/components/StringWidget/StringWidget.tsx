import * as React from 'react'
import TextField from 'material-ui/TextField'

import ConfigurationWidget from 'components/ConfigurationWidget/ConfigurationWidget'
import { ConfigurationOption } from 'snowflake-remoting'
import { ConfigurationKey } from 'support/ConfigurationKey'

type StringWidgetProps = {
  stringOption: ConfigurationOption,
  configkey: ConfigurationKey,
  isLoading: boolean,
  onValueChange: (newValue: string) => void
}

type StringWidgetState = {
  value: string
}
class StringWidget extends React.Component<StringWidgetProps, StringWidgetState> {
  constructor(props) {
    super(props)
    this.state = {
      value: ''
    }
  }

  componentDidMount () {
    this.setState({value: this.props.stringOption.Value.Value as string})
  }

  componentWillReceiveProps () {
    this.setState({value: this.props.stringOption.Value.Value as string})
  }

  valueChangeHandler = (event: Event) => {
    this.setState({value: (event.target as any).value as string})
  }

  postValueHandler = (event: Event) => {
    this.props.onValueChange(this.state.value)
  }

  enterKeyHandler = (event: KeyboardEvent) => {
    if(event.key === 'Enter') {
      this.props.onValueChange(this.state.value)
    }
  }
  render () {
    return (
    <ConfigurationWidget name={this.props.stringOption.Descriptor.DisplayName}
      description={this.props.stringOption.Descriptor.Description} isLoading={this.props.isLoading}>
     <TextField helperText='Press Enter to confirm.' fullWidth value={this.state.value} onChange={this.valueChangeHandler} onBlur={this.postValueHandler} onKeyDown={this.enterKeyHandler}/>
    </ConfigurationWidget>
  )
  }
}

export default StringWidget
