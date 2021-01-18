//
//  Dog.swift
//  DogSpot
//
//  Created by Stephen Walsh on 13/01/2021.
//

import Foundation

class Image {
    var height: Float = 0.0
    var width: Float = 0.0
    var id: String = ""
    var urlString: String = ""
}

class Weight {
    var imperial: String = ""
    var metric: String = ""
}

class Dog {
    var life_span: String!
    var id: Int = 0
    var image: Image = Image()
    var name: String = ""
    var origin: String = ""
    var temperament: String = ""
    var weight: Weight = Weight()
}
