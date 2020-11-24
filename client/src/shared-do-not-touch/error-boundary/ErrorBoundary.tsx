import React, { Component } from 'react'

type ErrorBoundaryState = {
  hasError: boolean
  error?: Error
}

export class ErrorBoundary extends Component<{}, ErrorBoundaryState> {
  constructor(props: any) {
    super(props)
    this.state = { hasError: false }
  }

  static getDerivedStateFromError(error: Error) {
    return { hasError: true, error }
  }

  componentDidCatch(error: Error, info: any) {
    this.setState({ hasError: true, error })
    // You can also log the error to an error reporting service
    //logErrorToMyService(error, info)
  }

  render() {
    if (this.state.hasError) {
      const { message, name, stack } = this.state.error!

      return (
        <div>
          Name: {name}
          Message: {message}
          Stack: {stack}
        </div>
      )
    }
    return this.props.children
  }
}