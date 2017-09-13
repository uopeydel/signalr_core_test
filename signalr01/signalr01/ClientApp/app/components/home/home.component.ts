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
        this.transportType = signalR.TransportType.WebSockets;
        let logger = new signalR.ConsoleLogger(signalR.LogLevel.Information);
        let connectOption = new signalR.HttpClient();
        let http = new signalR.HttpConnection(`http://${document.location.host}/chat?Name='xxx'`, { transport: this.transportType, logging: logger, httpClient: connectOption });
        
        this.connection = new signalR.HubConnection(http);
        
        this.connection.onClosed = e => {
            if (e) {
                this.appendLine('Connection closed with error: ' + e, 'red');
            }
            else {
                this.appendLine('Disconnected', 'green');
            }
        };

        this.connection.on('SetUsersOnline', usersOnline => {
            usersOnline.forEach((user: any) => this.addUserOnline(user));
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
                //TODO:find old user is has left chat and remove it.
            });
        });

        this.connection.on('Send', (userName, message) => {
            this.messages.push(userName + ':' + message);
        });

        
        this.connection.start().catch(err => this.appendLine(err, 'red'));
        
    }

    users: Array<any> = new Array<any>();
    messages: Array<any> = new Array<any>();

    TextForSend: string = "";

    SendMessage() {
        this.connection.invoke('Send', this.TextForSend).catch(err => this.appendLine(err, 'red'));
    }
    
    appendLine(line: any, color?: any) {
        this.messages.push(line);
    };

    addUserOnline(user: any) {
        this.users.push(user.Name + " " + user.ConnectionId);
    }

    IdForTestGroup: string ="";
    DoGroup() {
        this.connection.send('Send2', [this.IdForTestGroup, this.TextForSend] ,"x").catch(err => this.appendLine(err, 'red'));
        //this.connection.invoke('Send2', [this.IdForTestGroup, this.TextForSend]).catch(err => this.appendLine(err, 'red'));
    }
}

class UserDetails {
    ConnectionId: string;
    Name: string;
}
