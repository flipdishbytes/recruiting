//
//  SectionTitleView.swift
//  DogSpot
//
//  Created by Stephen Walsh on 13/01/2021.
//

import UIKit

class SectionTitleView: UICollectionReusableView {
    
    var dotStrokeColor: CGColor = UIColor.white.cgColor
    
    var titleLabel: UILabel = {
        let label = UILabel(frame: .zero)
        label.font = TypeMachine().sectionTitleFont
        label.textAlignment = .left
        label.textColor = Palette().textBlack
        
        return label
    }()
    
    override init(frame: CGRect) {
        super.init(frame: frame)
        backgroundColor = .clear
        
        addSubview(titleLabel)
        titleLabel.translatesAutoresizingMaskIntoConstraints = false
        NSLayoutConstraint.activate([
            titleLabel.leadingAnchor.constraint(equalTo: leadingAnchor, constant: 26),
            titleLabel.centerYAnchor.constraint(equalTo: centerYAnchor)
        ])
    }
    
    required init?(coder: NSCoder) {
        fatalError("init(coder:) has not been implemented") }
    
    override func draw(_ rect: CGRect) {
        super.draw(rect)
        
        guard let ctx = UIGraphicsGetCurrentContext() else { return }
        ctx.setLineCap(.square)
        ctx.setStrokeColor(dotStrokeColor)
        ctx.setLineWidth(11)
        ctx.setFillColor(UIColor.clear.cgColor)
        
        var start = CGPoint(x: 30 + rect.minX, y: rect.maxY - (22))
        ctx.move(to: start)
        
        var final = CGPoint(x: start.x+1, y: start.y)
        ctx.addLine(to: final)
        
        ctx.strokePath()
    }
}
