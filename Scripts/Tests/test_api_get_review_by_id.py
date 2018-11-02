import api_schemas
import requests
import cerberus
import pytest
from cerberus import Validator

def f_test_review(review):
    v = Validator(api_schemas.review_schema)
    correct = v.validate(review)
    assert correct == True

def f_test_review_id(review, id):
    correct = True
    if review["id"] != id:
        correct = False

    assert correct == True

def f_test_status_code_200(request):
    assert request.status_code == 200


base_url = ''
reviews_path = "reviews"

# set global base_url variable
@pytest.mark.unit
def test_api_path(apiurl):
    global base_url
    base_url = apiurl

# Test single review
def test_review():
    id=1
    review_request = requests.get(base_url+reviews_path+"/"+str(id))
    f_test_status_code_200(review_request)
    review = review_request.json()
    f_test_review(review)
    f_test_review_id(review, id)