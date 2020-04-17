import * as React from "react";
import { Button, ButtonType, ComboBox } from "office-ui-fabric-react";
import Header from "./Header";
import HeroList, { HeroListItem } from "./HeroList";
import Progress from "./Progress";
/* global Button, Header, HeroList, HeroListItem, Progress */

export default class App extends React.Component {
  constructor(props, context) {
    super(props, context);
    this.state = {
      listItems: []
    };
  }

  componentDidMount() {
    this.setState({
      listItems: [
        {
          icon: "Ribbon",
          primaryText: "Achieve more with Office integration"
        },
        {
          icon: "Unlock",
          primaryText: "Unlock features and functionality"
        },
        {
          icon: "Design",
          primaryText: "Create and visualize like a pro"
        }
      ]
    });
  }

  click = async () => {
    this.getBotIntent(keyboardInput.value);
  };
  getBotIntent = async(message) =>{
    var uri = "https://localhost:44337/api/Conversational?message="+message;
    uri="https://localhost:44337/api/Conversational?message=test";
    uri = "https://emergingtech-api.azurewebsites.net/api/Conversational?message="+message;
    keyboardInput.value = uri;

    fetch(uri,
      {
        method: "POST",
        withCredentials: false,
        headers: {
            'Content-Type': 'application/json'
        }
      }
    )
    .then(res => {
      keyboardInput.value = res.statusText;
    })
    .then(
      (result) => {
        //keyboardInput.value = result.Base64Audio;
      },
      (error) => {
        keyboardInput.value = "Test -----" +  error;
        keyboardInput.value = res.statusText;
      }
    );
  };
  showSuccess(response){
    keyboardInput.value = "success";
  };
  showError(error){
    keyboardInput.value = "Test -----" +  error;
  };
  render() {
    // const { title, isOfficeInitialized } = this.props;

    // if (!isOfficeInitialized) {
    //   return (
    //     <Progress title={title} logo="assets/logo-filled.png" message="Please sideload your addin to see app body." />
    //   );
    // }

    return (
      <div className="emerging-main">
        {/* <Header logo="assets/logo-filled.png" title={this.props.title} message="Let's talk!" /> */}
        {/* <HeroList items={this.state.listItems}>
          <p className="ms-font-l">
            Modify the source files, then click <b>Run</b>.
          </p>

        </HeroList> */}
        
      <section className="emerging-header">
        <img width="60" height="60" src="assets/logo-filled.png" />
      </section>
        <div className="emerging-chatbox">
          <div className="column1">
            <div className="column1items">
              <img width="25" height="25" src="assets/mic_off.png"/>
              <img width="25" height="25" src="assets/keyboard.png"/>
              <select id="language">
                <option value="en-US">en-U</option>
                <option value="en-GB" selected="true">en-GB</option>
                <option value="en-CA">en-CA</option>
                <option value="en-IN">en-IN</option>
              </select>
            </div>
          </div>
          <div className="column2">
            <input id="keyboardInput" type="text"></input>
          </div>
          <div className="column3">
            {/* <Button
                className="submit"
                buttonType={ButtonType.hero}
                iconProps={{ iconName: "ChevronRight" }}
                onClick={this.click}
              >
            </Button> */}
            <button className="submit" onClick={this.click}>Send</button>
          </div>
        </div>
      </div>
    );
  }
}
