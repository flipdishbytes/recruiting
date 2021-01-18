//
//  ListItemCell.swift
//  DogSpot
//
//  Created by Stephen Walsh on 13/01/2021.
//

import UIKit

class ListItemCell: UICollectionViewCell {
    
    var disclosureImageView: UIImageView = {
        var imageView = UIImageView()
        imageView.contentMode = .scaleAspectFit
        imageView.image = UIImage(named: "disclosureIndicator")!
        return imageView
    }()
    
    var titleLabel: UILabel = {
        var label = UILabel(frame: .zero)
        label.textColor = Palette().textBlack
        label.font = TypeMachine().listItemFont
        label.textAlignment = .left
        
        return label
    }()
    
    override init(frame: CGRect) {
        super.init(frame: frame)
        
        addSubview(disclosureImageView)
        addSubview(titleLabel)
        disclosureImageView.translatesAutoresizingMaskIntoConstraints = false
        
        titleLabel.translatesAutoresizingMaskIntoConstraints = false

        NSLayoutConstraint.activate([
            titleLabel.leadingAnchor.constraint(equalTo: leadingAnchor, constant: 26.0),
            titleLabel.centerYAnchor.constraint(equalTo: centerYAnchor),
            
            disclosureImageView.centerYAnchor.constraint(equalTo: centerYAnchor),
            disclosureImageView.trailingAnchor.constraint(equalTo: trailingAnchor, constant: -26),
            disclosureImageView.widthAnchor.constraint(equalToConstant: 40),
            disclosureImageView.heightAnchor.constraint(equalToConstant: 20),
        ])
    }
    
    required init?(coder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }
}
