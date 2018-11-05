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

def f_test_review_input(review, companyId, userId, stars, writtenReview, anonymous):
    correct = True
    if review["companyId"] != companyId:
        correct = False

    if review["userId"] != userId:
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


base_url = 'http://localhost:1221/'
reviews_path = "reviews"

# set global base_url variable
# def test_api_path(apiurl):
#     global base_url
#     base_url = apiurl

# Test review update

companyId = 1
userId = 1
stars = 3
writtenReview = "Dit is een goed stagebedrijf, prima begeleiding."
anonymous = 0

payload = {
    'companyId': companyId,
    'userId': userId,
    'stars': stars,
    'writtenReview': writtenReview,
	'anonymous': anonymous,
    }
review_request = requests.post(base_url+reviews_path, json=payload)
f_test_status_code_200(review_request)
review = review_request.json()
f_test_review(review)
f_test_review_input(review, companyId, userId, stars, writtenReview, anonymous)