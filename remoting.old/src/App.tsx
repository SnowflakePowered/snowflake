import * as React from "react"
import { Hello } from "./components/Hello"

export default class App extends React.Component<any, any> {
    public render() {
        return (
            <Hello compiler="Typescript" framework="React" />
        )
    }
}
