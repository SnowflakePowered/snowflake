import * as React from "react"
import * as Games from "../remoting/Games"
import * as Stone from "../remoting/Stone"

export interface HelloProps { 
    compiler: string
    framework: string 
}

// 'HelloProps' describes the shape of props.
// State is never set so we use the 'undefined' type.
export class Hello extends React.Component<HelloProps, undefined> {
    render() {
        return <h1>Nothing is working! {this.props.compiler} and {this.props.framework}!</h1>
    }

    componentDidMount() {
        this.getGame()
        console.log("hello")
    }

    async getGame() {
        let games = await Stone.getPlatforms()
        console.log(games)
    }
}