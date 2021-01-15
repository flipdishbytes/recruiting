//
//  TypeMachine.swift
//  DogSpot
//
//  Created by Stephen Walsh on 13/01/2021.
//

import UIKit

class TypeMachine {
    
    var titleFont: UIFont {
        var descriptor = UIFontDescriptor
            .preferredFontDescriptor(withTextStyle: .largeTitle)
            .withDesign(.serif)!
        
        return UIFont(descriptor: descriptor, size: 34)
    }
    
    var listItemFont: UIFont {
        let descriptor = UIFontDescriptor
            .preferredFontDescriptor(withTextStyle: .body)
            .withDesign(.rounded)!
        return UIFont(descriptor: descriptor, size: 21)
    }
    
    var sectionTitleFont: UIFont {
        
        let traits = [
            UIFontDescriptor.AttributeName.traits:
                [
                    UIFontDescriptor.TraitKey.weight: UIFont.Weight.medium
                ]
        ]
        
         let descriptor = UIFontDescriptor
            .preferredFontDescriptor(withTextStyle: .largeTitle)
            .withDesign(.serif)!
            .addingAttributes(traits)
        return UIFont(descriptor: descriptor, size: 21)
    }
    
    var cardFont: UIFont {
        let descriptor = UIFontDescriptor
                .preferredFontDescriptor(withTextStyle: .body)
                .withDesign(.rounded)!
        return UIFont(descriptor: descriptor, size: 17)
    }
    
    var bodyFont: UIFont {
        let descriptor = UIFontDescriptor
                .preferredFontDescriptor(withTextStyle: .body)
                .withDesign(.rounded)!
        return UIFont(descriptor: descriptor, size: 15)
    }
}
