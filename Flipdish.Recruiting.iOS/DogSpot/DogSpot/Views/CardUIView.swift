//
//  CardUIView.swift
//  DogSpot
//
//  Created by Stephen Walsh on 13/01/2021.
//

import UIKit

class CardUIView: UIView {
    
    override init(frame: CGRect) {
        super.init(frame: frame)
    }
    
    required init?(coder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }
    
    func setup() {
        var imageView = UIImageView()
        imageView.image = #imageLiteral(resourceName: "DrawThisInstead")
        imageView.contentMode = .scaleAspectFit
        imageView.clipsToBounds = false
        addSubview(imageView)
        imageView.translatesAutoresizingMaskIntoConstraints = false
    }
    
}
