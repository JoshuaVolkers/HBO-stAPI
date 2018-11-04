import requests
import cerberus
import pytest
import uuid
from cerberus import Validator
import json
import jwt

users_path = "users"

# arrange tests
def f_test_token(token):
    correct = isinstance(token, str)
    assert correct == True

def f_test_refresh_token_not_empty(refreshToken):
    correct = isinstance(refreshToken, str)
    assert correct == True

def f_test_refresh_token_matches_old_refresh_token(oldRefreshToken, newRefreshToken):
    correct = oldRefreshToken == newRefreshToken
    assert correct == True

def f_test_status_code_200(request):
    assert request.status_code == 200

def f_test_status_code_400(request):
    assert request.status_code == 400

def f_test_status_code_404(request):
    assert request.status_code == 404

# set global base_url variable
base_url = ''
def test_api_path(apiurl):
    global base_url
    base_url = apiurl

decodedToken = {}
refreshedDecodedToken = {}

# login with valid credentials
@pytest.mark.unit
def test_valid_jwt_token(studentemail, studentpass, jwtKey, jwtIssuer, jwtAudience):
    payload = {'emailAddress': studentemail, 'password': studentpass}
    login_request = requests.post(base_url+users_path+"/login", json=payload)
    f_test_status_code_200(login_request)
    token = login_request.json()
    f_test_token(token)

    global decodedToken
    decodedToken = jwt.decode(token, jwtKey, issuer = jwtIssuer, audience = jwtAudience, algorithms=['HS256'])

# test refresh token
def test_refresh_token(jwtKey, jwtIssuer, jwtAudience):
    refreshToken = decodedToken['refreshToken']
    f_test_refresh_token_not_empty(refreshToken)

    refreshedToken_request = requests.get(base_url+users_path+"token/"+refreshToken+"/refresh")
    f_test_status_code_200(refreshedToken_request)
    newToken = refreshedToken_request.json()
    f_test_token(newToken)

    global refreshedDecodedToken
    refreshedDecodedToken = jwt.decode(newToken, jwtKey, issuer = jwtIssuer, audience = jwtAudience, algorithms=['HS256'])

    newRefreshToken = refreshedDecodedToken['refreshToken']
    f_test_refresh_token_not_empty(newRefreshToken)
  
    f_test_refresh_token_matches_old_refresh_token(refreshToken, newRefreshToken)

# test revoking of existing token
def test_revoke_token():
    refreshToken = refreshedDecodedToken['refreshToken']

    revoke_refresh_token_request = requests.delete(base_url+users_path+"token/"+refreshToken+"/revoke")
    f_test_status_code_200(revoke_refresh_token_request)

# test refreshing invalid token and revoked token
def test_invalid_requests_refresh():
    invalidGuid_refreshedToken_request = requests.get(base_url+users_path+"token/DitIsGeenGUIDDusDeHeleMeukGaatOpZijnPlaat/refresh")
    f_test_status_code_400(invalidGuid_refreshedToken_request)

    revokedToken = refreshedDecodedToken['refreshToken']
    revoked_refresh_token_request = requests.get(base_url+users_path+"token/"+revokedToken+"/refresh")
    f_test_status_code_404(revoked_refresh_token_request)

def test_invalid_requests_revoke():
    invalidGuid_revokeToken_request = requests.delete(base_url+users_path+"token/DitIsGeenGUIDDusDeHeleMeukGaatOpZijnPlaat/revoke")
    f_test_status_code_400(invalidGuid_revokeToken_request)

    revokedToken = refreshedDecodedToken['refreshToken']
    revoked_revoke_token_request = requests.delete(base_url+users_path+"token/"+revokedToken+"/revoke")
    f_test_status_code_200(revoked_revoke_token_request)
