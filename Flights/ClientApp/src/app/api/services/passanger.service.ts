/* tslint:disable */
/* eslint-disable */
import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpContext } from '@angular/common/http';
import { BaseService } from '../base-service';
import { ApiConfiguration } from '../api-configuration';
import { StrictHttpResponse } from '../strict-http-response';
import { RequestBuilder } from '../request-builder';
import { Observable } from 'rxjs';
import { map, filter } from 'rxjs/operators';

import { NewPassengerDto } from '../models/new-passenger-dto';
import { PassangerRm } from '../models/passanger-rm';

@Injectable({
  providedIn: 'root',
})
export class PassangerService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * Path part for operation registerPassanger
   */
  static readonly RegisterPassangerPath = '/Passanger';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `registerPassanger()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  registerPassanger$Response(params?: {
    body?: NewPassengerDto
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, PassangerService.RegisterPassangerPath, 'post');
    if (params) {
      rb.body(params.body, 'application/*+json');
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: '*/*',
      context: context
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return (r as HttpResponse<any>).clone({ body: undefined }) as StrictHttpResponse<void>;
      })
    );
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `registerPassanger$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  registerPassanger(params?: {
    body?: NewPassengerDto
  },
  context?: HttpContext

): Observable<void> {

    return this.registerPassanger$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation findPassanger
   */
  static readonly FindPassangerPath = '/Passanger/{email}';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `findPassanger$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  findPassanger$Plain$Response(params: {
    email: string;
  },
  context?: HttpContext

): Observable<StrictHttpResponse<PassangerRm>> {

    const rb = new RequestBuilder(this.rootUrl, PassangerService.FindPassangerPath, 'get');
    if (params) {
      rb.path('email', params.email, {});
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain',
      context: context
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<PassangerRm>;
      })
    );
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `findPassanger$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  findPassanger$Plain(params: {
    email: string;
  },
  context?: HttpContext

): Observable<PassangerRm> {

    return this.findPassanger$Plain$Response(params,context).pipe(
      map((r: StrictHttpResponse<PassangerRm>) => r.body as PassangerRm)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `findPassanger()` instead.
   *
   * This method doesn't expect any request body.
   */
  findPassanger$Response(params: {
    email: string;
  },
  context?: HttpContext

): Observable<StrictHttpResponse<PassangerRm>> {

    const rb = new RequestBuilder(this.rootUrl, PassangerService.FindPassangerPath, 'get');
    if (params) {
      rb.path('email', params.email, {});
    }

    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'text/json',
      context: context
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<PassangerRm>;
      })
    );
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `findPassanger$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  findPassanger(params: {
    email: string;
  },
  context?: HttpContext

): Observable<PassangerRm> {

    return this.findPassanger$Response(params,context).pipe(
      map((r: StrictHttpResponse<PassangerRm>) => r.body as PassangerRm)
    );
  }

}
