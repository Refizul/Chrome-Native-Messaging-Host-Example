// Copyright 2013 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

let message;
let port = null;

function appendMessage(text) {
  document.getElementById('response').innerHTML += '<p>' + text + '</p>';
}

function updateUiState() {
  if (port) {
    document.getElementById('connect-button').style.display = 'none';
    document.getElementById('input-text').style.display = 'block';
    document.getElementById('send-message-button').style.display = 'block';
  } else {
    document.getElementById('connect-button').style.display = 'block';
    document.getElementById('input-text').style.display = 'none';
    document.getElementById('send-message-button').style.display = 'none';
  }
}

function sendNativeMessage() {
  message = { text: document.getElementById('input-text').value };
  port.postMessage(message);
  appendMessage('Sent message: <b>' + JSON.stringify(message) + '</b>');
}

function onNativeMessage(message) {
  appendMessage('Received message: <b>' + JSON.stringify(message) + '</b>');

  try {
    var command = JSON.parse(JSON.stringify(message));
    if(!command.exec) return;
    console.debug(command.exec);
    if(command.exec == 'bringTabToFront') {
      bringTabToFront(command.param);
    }
  } catch(ex) {
    console.error(ex.toString());
  }
}

async function bringTabToFront(titleSearchString) {
  var mytabs = await chrome.tabs.query({'title': titleSearchString});
  if(mytabs.length > 0) {
    chrome.tabs.update(mytabs[0].id,{active:true});
  } else {
    console.debug("Tab not found!");
  }
}

function onDisconnected() {
  appendMessage('Failed to connect: ' + chrome.runtime.lastError.message);
  port = null;
  updateUiState();
}

function connect() {
  const hostName = 'com.google.chrome.example.echo';
  appendMessage('Connecting to native messaging host <b>' + hostName + '</b>');
  port = chrome.runtime.connectNative(hostName);
  port.onMessage.addListener(onNativeMessage);
  port.onDisconnect.addListener(onDisconnected);
  updateUiState();
}

document.addEventListener('DOMContentLoaded', function () {
  document.getElementById('connect-button').addEventListener('click', connect);
  document
    .getElementById('send-message-button')
    .addEventListener('click', sendNativeMessage);
  updateUiState();
});