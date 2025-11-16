// // // import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection } from '@angular/core';
// // // import { provideRouter } from '@angular/router';

// // // import { routes } from './app.routes';
// // // import { provideClientHydration, withEventReplay } from '@angular/platform-browser';

// // // export const appConfig: ApplicationConfig = {
// // //   providers: [
// // //     provideBrowserGlobalErrorListeners(),
// // //     provideZonelessChangeDetection(),
// // //     provideRouter(routes), provideClientHydration(withEventReplay())
// // //   ]
// // // };

// // import {
// //   ApplicationConfig,
// //   provideBrowserGlobalErrorListeners,
// //   provideZonelessChangeDetection
// // } from '@angular/core';
// // import { provideRouter } from '@angular/router';
// // import {
// //   provideClientHydration,
// //   withEventReplay
// // } from '@angular/platform-browser';
// // import { provideHttpClient } from '@angular/common/http';

// // import { routes } from './app.routes';

// // export const appConfig: ApplicationConfig = {
// //   providers: [
// //     provideBrowserGlobalErrorListeners(),
// //     provideZonelessChangeDetection(),
// //     provideRouter(routes),
// //     provideClientHydration(withEventReplay()),
// //     provideHttpClient()
// //   ]
// // };


// // src/app/app.config.ts
// import { ApplicationConfig, provideZonelessChangeDetection  } from '@angular/core';
// import { provideRouter } from '@angular/router';
// import { routes } from './app.routes';

// export const appConfig: ApplicationConfig = {
//   providers: [
//     provideZonelessChangeDetection(),
//     provideRouter(routes)
//   ]
// };

import {
  ApplicationConfig,
  provideZonelessChangeDetection
} from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZonelessChangeDetection(),
    provideRouter(routes),
    provideHttpClient(),  
  ],
};
