user_location_schema = {
                    'countryName' : {'type' : 'string', 'nullable' : True},
                    'countryId' : {'type' : 'integer'},
                    'city' : {'type' : 'string', 'nullable' : True},
                    'longitude' : {'type' : 'float'},
                    'latitude' : {'type' : 'float'}
                    }

location_schema = {
                    'zipCode' : {'type' : 'string'},
                    'street' : {'type' : 'string'},
                    'houseNumber' : {'type' : 'integer'},
                    'houseNumberAdditions' : {'type' : 'string'},
                    'range' : {'type' : 'float'},
                    'countryName' : {'type' : 'string', 'nullable' : True},
                    'countryId' : {'type' : 'integer'},
                    'city' : {'type' : 'string'},
                    'longitude' : {'type' : 'float'},
                    'latitude' : {'type' : 'float'}
                    }

user_schema = {
                'id' : {'type' : 'integer'},
                'niceName' : {'type' : 'string'},
                'emailAddress' : {'type' : 'string'},
                'role' : {'type' : 'integer'},
                'active' : {'type' : 'boolean'},
                'educationalAttainmentId' : {'type' : 'integer'},
                'educationalAttainment' : {'type' : 'string', 'nullable' : True},
                'educationId' : {'type' : 'integer'},
                'education' : {'type' : 'string', 'nullable' : True},
                'favoriteVacancies' : {'type' : 'list', 'schema' : {'type' : 'integer'}, 'nullable' : True},
                'reviews' : {'type' : 'list', 'schema' : {'type' : 'integer'}, 'nullable' : True},
                'preferredLanguageId' : {'type' : 'integer'},
                'preferredLanguage' : {'type' : 'string', 'nullable' : True},
                'location' : {'type' : 'dict', 'schema' : user_location_schema}
                }

company_schema = {
                    'description' : {'type' : 'string'}, 
                    'emailAddress' : {'type' : 'string'},
                    'website' : {'type' : 'string'}, 
                    'socialLinkLinkedin' : {'type' : 'string'}, 
                    'id' : {'type' : 'integer'}, 
                    'name' : {'type' : 'string'}, 
                    'logoPath' : {'type' : 'string'}, 
                    'averageReviewStars' : {'type' : 'float'},
                    'majors' : {'type' : 'string', 'nullable' : True},
                    'location' : {'type' : 'dict', 'schema' : location_schema}
                    }

base_company_schema = {
                    'id' : {'type' : 'integer'}, 
                    'name' : {'type' : 'string'}, 
                    'logoPath' : {'type' : 'string'}, 
                    'averageReviewStars' : {'type' : 'float'},
                    'majors' : {'type' : 'string', 'nullable' : True},
                    'location' : {'type' : 'dict', 'schema' : location_schema}
                    }

company_review_schema = {
                    'id' : {'type' : 'integer'},
                    'companyId' : {'type' : 'integer'},
                    'stars' : {'type' : 'integer'},
                    'writtenReview' : {'type' : 'string'},
                    'nameReviewer' : {'type' : 'string'},
                    'reviewDate' : {'type' : 'string'}
                }

jwt_token_schema = {
                'id' : {'type' : 'string'},
                'authToken' : {'type' : 'string'},
                
}