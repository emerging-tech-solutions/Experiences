// import * as React from "react";

// export default class ConversationList extends React.Component {
//   render() {
//     const { children, items, message } = this.props;

//     const listItems = items.map((item, index) => (
//       <li className="ms-ListItem" key={index}>
//         <i className={`ms-Icon ms-Icon--${item.icon}`}></i>
//         <span className="ms-font-m ms-fontColor-neutralPrimary">{item.primaryText}</span>
//       </li>
//     ));
//     return (
//       <main className="ms-welcome__main">
//         <h2 className="ms-font-xl ms-fontWeight-semilight ms-fontColor-neutralPrimary ms-u-slideUpIn20">{message}</h2>
//         <ul className="ms-List ms-welcome__features ms-u-slideUpIn10">{listItems}</ul>
//         {children}
//       </main>
//     );
//   }
// }
import * as React from "react";

export default class ConversationList extends React.Component {
  render() {
    const { children, items, message } = this.props;

    const listItems = items.map((item, index) => (
      <div  className="ConversationItem" key={index}>
        {if }
        {/* (if (item.isAI)){
        <div className="ConversationAIIcon"></div>
        } */}
        <div className="Conversation">{item.Conversation}</div>
      </div>
    ));
    return (
      <main className="ConversationBackground">
        <h2 >{message}</h2>
        <ul >{listItems}</ul>
        {children}
      </main>
    );
  }
}
