import { TestBed } from '@angular/core/testing';
import { AuthService } from './auth.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [AuthService]
    });
    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  it('should login and store token', () => {
    service.login('user', 'pass').subscribe();

    const req = httpMock.expectOne('https://localhost:5001/api/auth/login');
    expect(req.request.method).toBe('POST');

    req.flush({ accessToken: 'fakeToken', refreshToken: 'fakeRefresh' });

    expect(service.token).toBe('fakeToken');
  });
});
