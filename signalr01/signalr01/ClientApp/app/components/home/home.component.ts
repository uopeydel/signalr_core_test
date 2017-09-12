import { Component ,OnInit} from '@angular/core';
import { ConsoleLogger, LogLevel, HttpConnection, HubConnection,TransportType } from '@aspnet/signalr-client';

@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {

    constructor() {

    }

    ngOnInit() {
        
    }

    doSigNalR()
    {
        //console.log('HomeComponent');
        let transportType = /*TransportType[this.getParameterByName('transport','')] ||*/ /*TransportType.WebSockets*/  TransportType.WebSockets;
        let logger = new ConsoleLogger(LogLevel.Information);
        let http = new HttpConnection(`http://${document.location.host}/chat`, { transport: transportType, logging: logger });
        console.log('http',http);

        let connection = new HubConnection(http/*, logger*/);
        //let connection = new HubConnection('http://localhost:58141/chat');
        console.log('connection',connection);
        console.log('HubConnection');
        connection.on('SetUsersOnline', (usersOnline: any) => {
            console.log('SetUsersOnline');
            usersOnline.forEach((user: any) => this.addUserOnline(user));
            });
    }

    users : any;
    addUserOnline(user: any) {
        console.log('show user');
        console.log(user);
    }

    getParameterByName(name: string, url: string) {
        if (!url || url == null || url == '') {
            url = window.location.href;
        }
        name = name.replace(/[\[\]]/g, "\\$&");
        let regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)");
        let results = regex.exec(url);
        if (!results)
        {
            return null;
        }
        if (!results[2])
        {
            return '';
        }
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    }
}

class UserDetails {
    ConnectionId: string;
    Name: string;
}