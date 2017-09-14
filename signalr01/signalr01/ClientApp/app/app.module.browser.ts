import { NgModule, InjectionToken } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppModuleShared } from './app.module.shared';
import { AppComponent } from './components/app/app.component';

export const USER_IDENTITY_NAME_TOKEN = new InjectionToken('userIdentityName');


@NgModule({
    bootstrap: [ AppComponent ],
    imports: [
        BrowserModule,
        AppModuleShared
    ],
    providers: [
        { provide: 'BASE_URL', useFactory: getBaseUrl },
        { provide: USER_IDENTITY_NAME_TOKEN, useValue: "x" }
    ]
})
export class AppModule {

}

export function getBaseUrl() {
    return document.getElementsByTagName('base')[0].href;
}

