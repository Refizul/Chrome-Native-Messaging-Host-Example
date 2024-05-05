
async function bringTabToFront(title) {
    var mytabs = await chrome.tabs.query({'title': title});
    if(mytabs.length > 0) {
      chrome.tabs.update(mytabs[0].id,{active:true});
    } else {
      console.debug("Tab not found!");
    }
  }