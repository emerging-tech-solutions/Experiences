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
    
    this.url = "data:audio/ogg;base64,";
    this.audio = new Audio(this.url);
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
        },
        {
          icon: "Design",
          primaryText: "Create and visualize like a pro"
        },
        {
          icon: "Design",
          primaryText: "Create and visualize like a pro"
        },
        {
          icon: "Design",
          primaryText: "Create and visualize like a pro"
        },
        {
          icon: "Design",
          primaryText: "Create and visualize like a pro"
        },
        {
          icon: "Design",
          primaryText: "Create and visualize like a pro"
        },
        {
          icon: "Design",
          primaryText: "Create and visualize like a pro"
        },
        {
          icon: "Design",
          primaryText: "Create and visualize like a pro"
        },
        {
          icon: "Design",
          primaryText: "Create and visualize like a pro"
        },
        {
          icon: "Design",
          primaryText: "Create and visualize like a pro"
        },
        {
          icon: "Design",
          primaryText: "Create and visualize like a pro"
        },
        {
          icon: "Design",
          primaryText: "Create and visualize like a pro"
        },
        {
          icon: "Design",
          primaryText: "Create and visualize like a pro"
        },
        {
          icon: "Design",
          primaryText: "Create and visualize like a pro"
        },
        {
          icon: "Design",
          primaryText: "Create and visualize like a pro"
        },
        {
          icon: "Design",
          primaryText: "Create and visualize like a pro"
        },
        {
          icon: "Design",
          primaryText: "Create and visualize like a pro"
        },
        {
          icon: "Design",
          primaryText: "Create and visualize like a pro"
        },
        {
          icon: "Design",
          primaryText: "Create and visualize like a pro"
        },
        {
          icon: "Design",
          primaryText: "Create and visualize like a pro"
        },
        {
          icon: "Design",
          primaryText: "Create and visualize like a pro"
        },
        {
          icon: "Design",
          primaryText: "Create and visualize like a pro"
        },
        {
          icon: "Design",
          primaryText: "Create and visualize like a pro"
        },
        {
          icon: "Design",
          primaryText: "Create and visualize like a pro"
        },
        {
          icon: "Design",
          primaryText: "Create and visualize like a pro"
        }
      ]
    });
  }
  play = () => {
    this.setState({ play: true, pause: false })
      this.audio.play();
    }
    
    pause = () => {
    this.setState({ play: false, pause: true })
      this.audio.pause();
    }
  click = async () => {
    this.getBotIntent(keyboardInput.value);
  };
  getBotIntent = async(message) =>{
    var uri = "https://localhost:44337/api/Conversational?message="+message;
    uri = "https://emergingtech-api.azurewebsites.net/api/Conversational?message="+message;

    fetch(uri,
      {
        method: "POST",
        withCredentials: false,
        headers: {
            'Content-Type': 'application/json'
        }
      }
    )  
  .then(async response => {
            const data = await response.json();

            // check for error response
            if (!response.ok) {
                // get error message from body or default to response status
                const error = (data && data.message) || response.status;
                return Promise.reject(error);
            }
            else{
              this.url = "data:audio/ogg;base64,"+data.base64Audio;
              this.audio = new Audio(this.url);
             this.play();
            }
        })
        .catch(error => {
            //this.setState({ errorMessage: error });
            console.error('There was an error!', error);
        });
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
      
        <audio id="voiceAudio" src="{this.state.audioURL}" autoplay></audio> 
        <section className="emerging-header">
          <img width="60" height="60" src="assets/logo-filled.png" />
        </section>
        
        <div className="emerging-conversations">
        <HeroList items={this.state.listItems}>
          <p className="ms-font-l">
            Modify the source files, then click <b>Run</b>.
          </p>

        </HeroList> 
        </div>
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
