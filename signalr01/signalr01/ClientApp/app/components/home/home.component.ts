import { Component, OnInit } from '@angular/core';
import * as signalR from '@aspnet/signalr-client';

@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})

export class HomeComponent implements OnInit {

    constructor() {

    }

    ngOnInit() {

    }

    connection: signalR.HubConnection;
    transportType: signalR.TransportType;


    doSigNalR() {
        //console.log('HomeComponent'); 
        this.transportType = /*TransportType[this.getParameterByName('transport','')] ||*/ /*TransportType.WebSockets*/  signalR.TransportType.WebSockets;
        let logger = new signalR.ConsoleLogger(signalR.LogLevel.Information);
        let http = new signalR.HttpConnection(`http://${document.location.host}/chat`, { transport: this.transportType, logging: logger });
        console.log('http', http);

        this.connection = new signalR.HubConnection(http/*, logger*/);
        //let connection = new HubConnection('http://localhost:58141/chat'); 
        console.log('connection', this.connection);
        console.log('HubConnection');



        this.connection.onClosed = e => {
            if (e) {
                this.appendLine('Connection closed with error: ' + e, 'red');
            }
            else {
                this.appendLine('Disconnected', 'green');
            }
        };

        this.connection.on('SetUsersOnline', usersOnline => {
            usersOnline.forEach((user:any) => this.addUserOnline(user));
        });

        this.connection.on('UsersJoined', users => {
            users.forEach((user: any) => {
                this.appendLine('User ' + user.Name + ' joined the chat');
                this.addUserOnline(user);
            });
        });

        this.connection.on('UsersLeft', users => {
            users.forEach((user: any) => {
                this.appendLine('User ' + user.Name + ' left the chat');
                //document.getElementById(user.ConnectionId).outerHTML = '';
            });
        });

        this.connection.on('Send', (userName, message) => {
            //var nameElement = document.createElement('b');
            //nameElement.innerText = userName + ':';

            //var msgElement = document.createElement('span');
            //msgElement.innerText = ' ' + message;

            //var child = document.createElement('li');
            //child.appendChild(nameElement);
            //child.appendChild(msgElement);
            //document.getElementById('messages').appendChild(child);
            this.messages.push(userName + ':' + message);
        });

        this.connection.start().catch(err => this.appendLine(err, 'red'));


    }

    users: Array<any> = new Array<any>();
    messages: Array<any> = new Array<any>();

    SendMessage() {
        this.connection.invoke('Send', this.TextForSend).catch(err => this.appendLine(err, 'red'));
    }
    TextForSend: string = "";



    appendLine(line: any, color?: any) {
        //let child = document.createElement('li');
        //if (color) {
        //    child.style.color = color;
        //}
        //child.innerText = line;
        this.messages.push(line);
    };

    addUserOnline(user: any) {
        //if (document.getElementById(user.ConnectionId)) {
        //    return;
        //}
        //var userLi = document.createElement('li');
        //userLi.innerText = `${user.Name} (${user.ConnectionId})`;
        //userLi.id = user.ConnectionId;
        //document.getElementById('users').appendChild(userLi);
        this.users.push(user.Name + " " + user.ConnectionId);
    }

    getParameterByName(name: any, url: any) {
        if (!url) {
            url = window.location.href;
        }
        name = name.replace(/[\[\]]/g, "\\$&");
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    };
}

class UserDetails {
    ConnectionId: string;
    Name: string;
}
