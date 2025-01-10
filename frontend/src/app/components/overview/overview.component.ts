import { Component, OnInit, signal } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

@Component({
  selector: 'app-overview',
  imports: [],
  templateUrl: './overview.component.html',
  styleUrl: './overview.component.scss'
})
export class OverviewComponent implements OnInit{
  private hubConnection!: HubConnection;
  messages: Message[] = [];
  savedName: string = '';
  signedIn: boolean = false;
  pwdOk: boolean = false;
  clients = signal(0);

  checkPassword(pwd: string): void {
    if(pwd.length >= 5){
      this.pwdOk = true;
    }else{
      this.pwdOk = false;
    }
  }
  
  ngOnInit(): void {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:5000/hubs/chat')
      .build();
    this.registerChatters();
    this.hubConnection.start()
      .then(() => console.log('Connection started!'));
  }

  private registerChatters(): void {
    this.hubConnection.on('NewMessage', (name: string, msg: string, timestamp: string) => {
      this.messages.push({ name, msg, timestamp });
    });
    this.hubConnection.on('ClientConnected', (name: string) => {
      this.messages.push({ name: '', msg: `Client ${name} connected`, timestamp: `${new Date().toLocaleDateString()} ${new Date().toLocaleTimeString()}` });
    });
    this.hubConnection.on('ClientDisconnected', (name: string) => {
      this.messages.push({ name: '', msg: `Client ${name} disconnected`, timestamp: `${new Date().toLocaleDateString()} ${new Date().toLocaleTimeString()}` });
    });
    this.hubConnection.on('NrClientsChanged', (nr: number) => {
      this.clients.set(nr);
    });
  }

  sendMsg(msg: string): void {
    this.hubConnection.send('SendMessage', this.savedName, msg, '');
  }

  signIn(name: string, pwd: string): void {
    this.savedName = name;
    this.signedIn = true;
    this.hubConnection.send('SignIn', name, pwd);
    this.hubConnection.invoke('GetNrClients')
      .then((nr: number) => this.clients.set(nr));
  }

  signOut(): void {
    this.signedIn = false;
    this.hubConnection.send('SignOut');
  }
}

type Message = {
  name: string;
  msg: string;
  timestamp: string;
};
