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

    RegisterId: number = 10;
    RegisterClientType: number = 11;//MobileRq

    connection: signalR.HubConnection;
    transportType: signalR.TransportType;

    doSigNalR() {
        this.transportType = signalR.TransportType.WebSockets;
        let logger = new signalR.ConsoleLogger(signalR.LogLevel.Information);

        let connectOption = new signalR.HttpClient();
        console.log(connectOption.options);
        var Domain = `http://${document.location.host}/chat`;
        Domain = 'http://localhost:54021/NotificationHub';
        let http = new signalR.HttpConnection(Domain, { transport: this.transportType, logging: logger, httpClient: connectOption });

        this.connection = new signalR.HubConnection(http);

        this.connection.onClosed = e => {
            if (e) {
                this.appendLine('Connection closed with error: ' + e, 'red');
            }
            else {
                this.appendLine('Disconnected', 'green');
            }
        };

        //this.connection.on('SetUsersOnline', usersOnline => {
        //    usersOnline.forEach((user: any) => this.addUserOnline(user));
        //});

        //this.connection.on('UsersJoined', users => {
        //    users.forEach((user: any) => {
        //        this.appendLine('User ' + user.Name + ' joined the chat');
        //        this.addUserOnline(user);
        //    });
        //});

        //this.connection.on('UsersLeft', users => {
        //    users.forEach((user: any) => {
        //        this.appendLine('User ' + user.Name + ' left the chat');
        //        //TODO:find old user is has left chat and remove it.
        //    });
        //});

        this.connection.on('Send', (userName, message) => {
            this.messages.push(userName + ':' + message);
        });

        this.connection.start().then(data => {
            this.connection.send('RegisterUserMap', this.RegisterId, this.RegisterClientType)
                .catch(err => {
                    this.connection.stop();
                    this.appendLine(err, 'red');
                });
        }).catch(err => {
            this.appendLine(err, 'red');
            this.connection.stop();
        });
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

    IdForTestGroup: string = "";
    DoGroup() {
        this.connection.send('Send2', ["xx", this.TextForSend], this.IdForTestGroup).catch(err => this.appendLine(err, 'red'));
        //this.connection.invoke('Send2', [this.IdForTestGroup, this.TextForSend]).catch(err => this.appendLine(err, 'red'));
    }

}

class UserDetails {
    ConnectionId: string;
    Name: string;
}
