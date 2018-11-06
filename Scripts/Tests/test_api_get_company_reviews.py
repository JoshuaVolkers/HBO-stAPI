import api_schemas
import requests
import cerberus
import pytest
from cerberus import Validator

def f_test_review(review):
    v = Validator(api_schemas.company_review_schema)
    correct = v.validate(review)
    assert correct == True

def f_test_status_code_200(request):
    assert request.status_code == 200



base_url = ''
companies_path = "companies"

# set global base_url variable
@pytest.mark.unit
def test_api_path(apiurl):
    global base_url
    base_url = apiurl

# Test review of company
def test_review_company():
    id=1
    company_review_request = requests.get(base_url+companies_path+"/"+str(id)+"/reviews")
    f_test_status_code_200(company_review_request)
    reviews = company_review_request.json()
    f_test_review(reviews[0])