import api_schemas
import requests
import cerberus
import pytest
import datetime
from cerberus import Validator
import json

def f_test_review(review):
    v = Validator(api_schemas.review_schema)
    correct = v.validate(review)
    assert correct == True

def f_test_review_input(review, companyId, stars, writtenReview, anonymous):
    correct = True
    if review["companyId"] != companyId:
        correct = False

    if review["stars"] != stars:
        correct = False

    if review["writtenReview"] != writtenReview:
        correct = False

    if review["anonymous"] != anonymous:
        correct = False

    assert correct == True

def f_test_status_code_200(request):
    assert request.status_code == 200

base_url = ''
reviews_path = "reviews"
users_path = "users"
token = {}
headers = ''

#set global base_url variable
def test_api_path(apiurl):
    global base_url
    base_url = apiurl

# login with student credentials
@pytest.mark.unit
def test_login(studentemail, studentpass):
    payload = {'emailAddress': studentemail, 'password': studentpass}
    login_request = requests.post(base_url+users_path+"/login", json=payload)
    f_test_status_code_200(login_request)
    token = login_request.json()

    global headers
    headers = {'Authorization' : 'Bearer '+token}

# Test review update
def test_post_new_review():
    companyId = 1
    stars = 3
    writtenReview = "TEST_ Dit is een goed stagebedrijf, prima begeleiding."
    anonymous = 0

    payload = {
        'companyId': companyId,
        'stars': stars,
        'writtenReview': writtenReview,
        'anonymous': anonymous,
        }
    review_request = requests.post(base_url+reviews_path, json=payload, headers=headers)
    f_test_status_code_200(review_request)
    review = review_request.json()
    f_test_review(review)
    f_test_review_input(review, companyId, stars, writtenReview, anonymous)