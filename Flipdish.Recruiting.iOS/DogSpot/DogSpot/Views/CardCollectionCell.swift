//
//  CardCollectionCell.swift
//  DogSpot
//
//  Created by Stephen Walsh on 13/01/2021.
//

import UIKit

protocol Delegate {
    func tapCardCell(indexPath: IndexPath)
}

class CardCollectionCell: UICollectionViewCell, UICollectionViewDataSource, UICollectionViewDelegateFlowLayout, UICollectionViewDelegate {
    
    
    
    var baseImageAPI: String = "https://cdn2.thedogapi.com/images/"
    
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        return items.count
    }
    
    var collectionView: UICollectionView!
    
    var delegate: Delegate!
    var items: [Dog]!
    
    override init(frame: CGRect) {
        
        super.init(frame: .zero)
        
        var layout = UICollectionViewFlowLayout()
        layout.scrollDirection = .horizontal
        self.collectionView = UICollectionView(frame: .zero, collectionViewLayout: layout)
        collectionView.register(CardItemCell.self, forCellWithReuseIdentifier: "CardItemCell")
        
        addSubview(collectionView)
        collectionView.translatesAutoresizingMaskIntoConstraints = false
        
        NSLayoutConstraint.activate([
            collectionView.leadingAnchor.constraint(equalTo: leadingAnchor),
            collectionView.topAnchor.constraint(equalTo: topAnchor),
            collectionView.trailingAnchor.constraint(equalTo: trailingAnchor),
            collectionView.bottomAnchor.constraint(equalTo: bottomAnchor),
        ])
        
        collectionView.backgroundColor = .clear
        collectionView.alwaysBounceHorizontal = true
        collectionView.delegate = self
        collectionView.contentInset = UIEdgeInsets(top: 0, left: 26, bottom: 0, right: 26)
        collectionView.dataSource = self
    }
    
    required init?(coder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }
    
    func numberOfSections(in collectionView: UICollectionView) -> Int {
        return 1
    }
    
    func collectionView(_ collectionView: UICollectionView,
                        cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        var cell = collectionView.dequeueReusableCell(withReuseIdentifier: "CardItemCell", for: indexPath) as! CardItemCell
        var item = items[indexPath.item]
        
        let url = URL(string: baseImageAPI + item.image.id + ".jpg")
        var session = URLSession(configuration: URLSessionConfiguration.default,
                                 delegate: cell, delegateQueue: nil)
        
        session.downloadTask(with: url!).resume()
        
        cell.titleLabel.text = item.name
        
        return cell
    }
    
    
    func collectionView(_ collectionView: UICollectionView,
                        layout collectionViewLayout: UICollectionViewLayout, minimumLineSpacingForSectionAt section: Int) -> CGFloat {
        return 18
    }
    
    func collectionView(_ collectionView: UICollectionView,
                        layout collectionViewLayout: UICollectionViewLayout,
                        sizeForItemAt indexPath: IndexPath) -> CGSize {
        return CGSize(width: 154, height: collectionView.frame.size.height)
    }
    
    func collectionView(_ collectionView: UICollectionView, didSelectItemAt indexPath: IndexPath) {
        delegate.tapCardCell(indexPath: indexPath)
    }
}

class CardItemCell: ListItemCell, URLSessionDownloadDelegate {
    
    var imageView: UIImageView = {
        
        var imageView = UIImageView()
        imageView.contentMode = .scaleAspectFill
        imageView.clipsToBounds = true
        imageView.backgroundColor = Palette().floatyBlue
        return imageView
    }()
    
    override init(frame: CGRect) {
        super.init(frame: frame)
        
        titleLabel.font = TypeMachine().cardFont
        titleLabel.textAlignment = .center
        titleLabel.textColor = Palette().textBlack
        
        titleLabel.removeFromSuperview()
        disclosureImageView.removeFromSuperview()
        
        var cardView = CardUIView(frame: .zero)
        cardView.setup()
        self.insertSubview(cardView, at: 0)
        
        addSubview(cardView)
        addSubview(titleLabel)
        addSubview(imageView)
        
        imageView.translatesAutoresizingMaskIntoConstraints = false;cardView.translatesAutoresizingMaskIntoConstraints=false;imageView.translatesAutoresizingMaskIntoConstraints=false;
        
        NSLayoutConstraint.activate([
            
            cardView.topAnchor.constraint(equalTo: topAnchor),
            cardView.trailingAnchor.constraint(equalTo: trailingAnchor),
            cardView.leadingAnchor.constraint(equalTo: leadingAnchor),
            cardView.bottomAnchor.constraint(equalTo: bottomAnchor, constant: -10),
            
            imageView.topAnchor.constraint(equalTo: topAnchor),
            imageView.trailingAnchor.constraint(equalTo: trailingAnchor),
            imageView.leadingAnchor.constraint(equalTo: leadingAnchor),
            imageView.heightAnchor.constraint(equalToConstant: 118),
            
            titleLabel.topAnchor.constraint(equalTo: imageView.bottomAnchor),
            titleLabel.leadingAnchor.constraint(equalTo: leadingAnchor),
            titleLabel.trailingAnchor.constraint(equalTo: trailingAnchor),
            titleLabel.bottomAnchor.constraint(equalTo: bottomAnchor)
            
        ])
        
        cardView.setNeedsDisplay()
    }
    
    required init?(coder: NSCoder)
    {
        
        
        fatalError("init(coder:) has not been implemented")
    }
    
    
    func urlSession(_ session: URLSession, downloadTask: URLSessionDownloadTask, didFinishDownloadingTo location: URL) {
        guard let data = try? Data(contentsOf: location),
              let image = UIImage(data: data) else { return }
        self.imageView.image = image
    }
}
