//
//  DogDetailsViewController.swift
//  DogSpot
//
//  Created by Stephen Walsh on 13/01/2021.
//

import UIKit

protocol DetailsDelegate {
    func didFavoriteDog(id: Int)
}

class DogDetailsViewController: UIViewController, URLSessionDownloadDelegate {
    
    var baseImageAPI: String = "https://cdn2.thedogapi.com/images/"

    
    var delegate: DetailsDelegate!
    var dog: Dog!
    var isFavorite: Bool = false
    
    var mainStackView: UIStackView!
    
    var button: UIButton!
    
    var dog_imageView: UIImageView!
    
    override func viewDidLoad() {
        
        mainStackView = UIStackView(frame: .zero)
        mainStackView.axis = .vertical
        mainStackView.distribution = .fillProportionally
        
        self.view.backgroundColor = Palette().backgroundGray
        title = dog.name
        
        
        self.button = UIButton(frame: .zero)
        self.button.setTitle("Favorite", for: .normal)
        self.button.setTitle("Unfavorite", for: .selected)
        button.backgroundColor = Palette().actionRed
        
        if isFavorite {
            self.button.isSelected = true;
        }
        
        button.translatesAutoresizingMaskIntoConstraints = false
        
        view.addSubview(button)
        self.dog_imageView = UIImageView()
        dog_imageView.translatesAutoresizingMaskIntoConstraints = false
        view.addSubview(dog_imageView)
        mainStackView.translatesAutoresizingMaskIntoConstraints = false
        
        view.addSubview(mainStackView)
        NSLayoutConstraint.activate([
        
            self.button.widthAnchor.constraint(equalToConstant: 132),
            self.button.heightAnchor.constraint(equalToConstant: 54),
            self.button.bottomAnchor.constraint(equalTo: view.bottomAnchor, constant: -26),
            self.button.leadingAnchor.constraint(equalTo: view.leadingAnchor, constant: 22),
            
            
            dog_imageView.widthAnchor.constraint(equalTo: view.widthAnchor, constant: 0),
            dog_imageView.centerXAnchor.constraint(equalTo: view.centerXAnchor, constant: 0),
            dog_imageView.leadingAnchor.constraint(equalTo: view.leadingAnchor, constant: 0),
            dog_imageView.trailingAnchor.constraint(equalTo: view.trailingAnchor, constant: 0),
            dog_imageView.heightAnchor.constraint(equalTo: view.heightAnchor, constant: -300),
            
            mainStackView.leadingAnchor.constraint(equalTo: view.leadingAnchor, constant: 26),
            mainStackView.trailingAnchor.constraint(equalTo: view.trailingAnchor, constant: -26),
            mainStackView.topAnchor.constraint(equalTo: dog_imageView.bottomAnchor, constant: 26),
            mainStackView.heightAnchor.constraint(equalToConstant: 200),
            
            mainStackView.bottomAnchor.constraint(equalTo: button.topAnchor, constant: -26)
        ])
        
        self.button.addTarget(self, action: #selector(button_touch), for: .touchUpInside)
        
        self.dog_imageView.backgroundColor = Palette().floatyBlue
        styleThebutton()
        
        
        
    }
    
    override func viewDidAppear(_ animated: Bool) {
        
        let url = URL(string: baseImageAPI + dog.image.id + ".jpg")
        var sess = URLSession(configuration: .default, delegate: self, delegateQueue: nil)
        sess.downloadTask(with: url!).resume()

        addattributes()
        
    }
    
    private func styleThebutton() {
        button.layer.cornerRadius = button.frame.size.height/2
        button.layer.masksToBounds = true
    }
    
    private func addattributes() {
        let origin = AttributeView(titleText: "Origin", subtitleText: dog.origin)
        var temperament = AttributeView(titleText: "Temperament", subtitleText: dog.temperament)
        var size = AttributeView(titleText: "Size", subtitleText: dog.weight.metric)
        var lifesp = AttributeView(titleText: "Life Span", subtitleText: dog.life_span + "years")
        let otherStack = UIStackView(arrangedSubviews: [size, lifesp])
        
        otherStack.axis = .horizontal
        otherStack.distribution = .fillEqually
        
        mainStackView.addArrangedSubview(origin)
        mainStackView.addArrangedSubview(temperament)
        mainStackView.addArrangedSubview(otherStack)
    }
    
    @objc private func button_touch() {
        delegate.didFavoriteDog(id: dog.id)
    }
    
    func urlSession(_ session: URLSession, downloadTask: URLSessionDownloadTask, didFinishDownloadingTo location: URL) {
        guard let data = try? Data(contentsOf: location),
              let image = UIImage(data: data) else { return }
        
        DispatchQueue.main.async {
            
            self.dog_imageView.image = image
            
        }
     
    }
    
}


class AttributeView: UIView {
    
    var titlelabel: UILabel?
    var subtitleLable: UILabel?
    
    init(titleText: String, subtitleText: String) {
        self.titlelabel = UILabel()
        self.subtitleLable = UILabel()
        self.titlelabel?.text = titleText
        self.subtitleLable?.text = subtitleText
        
        self.titlelabel?.font = TypeMachine().sectionTitleFont
        self.subtitleLable?.font = TypeMachine().bodyFont
        
        super.init(frame: .zero)
        
        addSubview(titlelabel!)
        addSubview(subtitleLable!)
        
        titlelabel?.translatesAutoresizingMaskIntoConstraints = false
        subtitleLable!.translatesAutoresizingMaskIntoConstraints = false
        
        NSLayoutConstraint.activate([
            titlelabel!.topAnchor.constraint(equalTo: topAnchor),
            titlelabel!.leadingAnchor.constraint(equalTo: leadingAnchor),
            titlelabel!.trailingAnchor.constraint(equalTo: trailingAnchor),
            
            subtitleLable!.topAnchor.constraint(equalTo: titlelabel!.bottomAnchor),
            subtitleLable!.leadingAnchor.constraint(equalTo: leadingAnchor),
            subtitleLable!.trailingAnchor.constraint(equalTo: trailingAnchor)
        
        ])
    }
    
    required init?(coder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }
}
